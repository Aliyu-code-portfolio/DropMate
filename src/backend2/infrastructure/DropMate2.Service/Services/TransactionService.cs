using AutoMapper;
using DropMate2.Application.Common;
using DropMate2.Application.ServiceContracts;
using DropMate2.Domain.Models;
using DropMate2.Shared.Dtos.Request;
using DropMate2.Shared.Dtos.Response;
using DropMate2.Shared.Exceptions.Sub;
using DropMate2.Shared.RequestFeatures;
using DropMate2.Shared.RequestFeatures.Common;


namespace DropMate2.Service.Services
{
    public class TransactionService : ITransactionService
    {
        public IUnitOfWork _unitOfWork;
        public IMapper _mapper;
        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CompleteTransaction(int packageId, bool isCompleted)
        {
            Transaction transactionExist = await GetTransactionWithPackageId(packageId, false) 
                ?? throw new TransactionNotFoundException(packageId);
            if(transactionExist.IsCompleted)
            {
                throw new TransactionCompletedException(transactionExist.Id);
            }
            Wallet walletToCredit;
            if (isCompleted)
            {
               walletToCredit = await _unitOfWork.WalletRepository.GetWalletByIdAsync(transactionExist.RecieverWalletID, false);
               transactionExist.IsCompleted = true;
            }
            else
            {
                walletToCredit = await _unitOfWork.WalletRepository.GetWalletByIdAsync(transactionExist.SenderWalletID, false);
                _unitOfWork.TransactionRepository.PermanentDeleteTransaction(transactionExist);
            }
            walletToCredit.Balance += transactionExist.PaymentAmount;
            _unitOfWork.WalletRepository.UpdateWallet(walletToCredit);
            _unitOfWork.TransactionRepository.UpdateTransaction(transactionExist);

            await _unitOfWork.SaveAsync();
        }

        public async Task<StandardResponse<TransactionResponseDto>> CreateTransaction(TransactionRequestDto transactionDto)
        {
            Transaction transactionExist = await GetTransactionWithPackageId(transactionDto.PackageId, false);
            if(transactionExist != null)
            {
                throw new TransactionAlreadyExistException( transactionDto.PackageId);
            }
            Transaction transaction = _mapper.Map<Transaction>(transactionDto);

            Wallet senderWallet = await GetAndEnsureWalletExists(transaction.SenderWalletID, "Sender");
            _ = await GetAndEnsureWalletExists(transaction.RecieverWalletID, "Receiver");

            EnsureSufficientFunds(senderWallet.Balance, transaction.PaymentAmount, transaction.PackageId);

            senderWallet.Balance -= transaction.PaymentAmount;
            _unitOfWork.TransactionRepository.CreateTransaction(transaction);
            _unitOfWork.WalletRepository.UpdateWallet(senderWallet);
            await _unitOfWork.SaveAsync();
            TransactionResponseDto responseDto = _mapper.Map<TransactionResponseDto>(transaction);
            return StandardResponse<TransactionResponseDto>.Success("Successfully created a transaction", responseDto, 201);
        }

        public async Task DeleteTransaction(int id)
        {
            Transaction transaction = await GetTransactionWithId(id, false);
            _unitOfWork.TransactionRepository.DeleteTransaction(transaction);  
            await _unitOfWork.SaveAsync();

        }

        public async Task<StandardResponse<(IEnumerable<TransactionResponseDto> transactions, MetaData metaData)>> GetAllTransaction(TransactionRequestParameters requestParameters, bool trackChanges)
        {
            PagedList<Transaction> transactions = await _unitOfWork.TransactionRepository
                .GetAllTransactionsAsync(requestParameters, trackChanges);
            IEnumerable<TransactionResponseDto> responseDtos = _mapper.Map<IEnumerable<TransactionResponseDto>>(transactions);
            return StandardResponse<(IEnumerable<TransactionResponseDto> transactions, MetaData metaData)>
                .Success("Successfully retrieved transactions",(responseDtos,transactions.MetaData));
        }

        public async Task<StandardResponse<(IEnumerable<TransactionResponseDto> transactions, MetaData metaData)>> GetAllUserTransaction(TransactionRequestParameters requestParameters, string userId, bool trackChanges)
        {
            PagedList<Transaction> transactions = await _unitOfWork.TransactionRepository
                .GetAllUserTransactionsAsync(requestParameters,userId, trackChanges);
            IEnumerable<TransactionResponseDto> responseDtos = _mapper.Map<IEnumerable<TransactionResponseDto>>(transactions);
            return StandardResponse<(IEnumerable<TransactionResponseDto> transactions, MetaData metaData)>
                .Success("Successfully retrieved transactions", (responseDtos, transactions.MetaData));
        }

        public async Task<StandardResponse<TransactionResponseDto>> GetTransactionById(int id, bool trackChanges)
        {
            Transaction transaction = await GetTransactionWithId(id, trackChanges);
            TransactionResponseDto responseDto = _mapper.Map<TransactionResponseDto>(transaction);
            return StandardResponse<TransactionResponseDto>.Success("Successfully retrieved transaction", responseDto);
        }

        public async Task UpdateTransaction(int id, TransactionRequestDto transactionDto)
        {
            _ = await GetTransactionWithId(id, false);
            Transaction transaction = _mapper.Map<Transaction>(transactionDto);
            _unitOfWork.TransactionRepository.UpdateTransaction(transaction);
            await _unitOfWork.SaveAsync();
        }
        private async Task<Transaction> GetTransactionWithId(int id, bool trackChanges)
        {
            return await _unitOfWork.TransactionRepository.GetTransactionByIdAsync(id, trackChanges)??
                throw new TransactionNotFoundException(id);
        }
        private async Task<Transaction> GetTransactionWithPackageId(int packageId, bool trackChanges)
        {
             return await _unitOfWork.TransactionRepository
                .GetTransactionByPackageIdAsync(packageId, false);
        }
        private async Task<Wallet> GetAndEnsureWalletExists(string walletId, string walletType)
        {
            Wallet wallet = await _unitOfWork.WalletRepository.GetWalletByIdAsync(walletId, false)
                ?? throw new WalletNotFoundException(walletId, walletType);

            return wallet;
        }

        private void EnsureSufficientFunds(decimal balance, decimal paymentAmount, int packageId)
        {
            if (balance - paymentAmount < 0)
            {
                throw new InsufficientFundFailedException(packageId);
            }
        }

        public async Task RefundPackagePayment(int packageId)
        {
            Transaction transaction = await _unitOfWork.TransactionRepository.GetTransactionByPackageIdAsync(packageId, false)
                ?? throw new TransactionNotFoundException(packageId);
            Wallet wallet = await _unitOfWork.WalletRepository.GetWalletByIdAsync(transaction.SenderWalletID, false)
                ?? throw new WalletNotFoundException(transaction.SenderWalletID);
            wallet.Balance += transaction.PaymentAmount;
            _unitOfWork.TransactionRepository.DeleteTransaction(transaction);
            _unitOfWork.WalletRepository.UpdateWallet(wallet);
            await _unitOfWork.SaveAsync();
        }
    }
}
