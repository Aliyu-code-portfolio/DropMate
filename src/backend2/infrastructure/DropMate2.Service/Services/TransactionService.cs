﻿using AutoMapper;
using DropMate.Application.Common;
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
            if (isCompleted)
            {
                Wallet creditWallet = await _unitOfWork.WalletRepository.GetWalletByIdAsync(transactionExist.RecieverWalletID, false);
                creditWallet.Balance += transactionExist.PaymentAmount;
                _unitOfWork.WalletRepository.UpdateWallet(creditWallet);
                transactionExist.IsCompleted = true;
                _unitOfWork.TransactionRepository.UpdateTransaction(transactionExist);
            }
            else
            {
                Wallet creditWallet = await _unitOfWork.WalletRepository.GetWalletByIdAsync(transactionExist.SenderWalletID, false);
                creditWallet.Balance += transactionExist.PaymentAmount;
                _unitOfWork.WalletRepository.UpdateWallet(creditWallet);
                _unitOfWork.TransactionRepository.PermanentDeleteTransaction(transactionExist);
            }
            await _unitOfWork.SaveAsync();
        }

        public async Task CreateTransaction(TransactionRequestDto transactionDto)
        {
            Transaction transactionExist = await GetTransactionWithPackageId(transactionDto.PackageId, false);
            if(transactionExist is not null)
            {
                throw new TransactionAlreadyExistException( transactionDto.PackageId);
            }
            Transaction transaction = _mapper.Map<Transaction>(transactionDto);
            Wallet debitWallet = await _unitOfWork.WalletRepository.GetWalletByIdAsync(transaction.SenderWalletID, false);
            //credit the traveler wallet after the package has been delivered
            if (debitWallet.Balance - transaction.PaymentAmount < 0)
            {
                throw new InsufficientFundFailedException(transaction.PackageId);
            }
            debitWallet.Balance -= transaction.PaymentAmount;
            _unitOfWork.TransactionRepository.CreateTransaction(transaction);
            await _unitOfWork.SaveAsync();
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
                .Success("Success",(responseDtos,transactions.MetaData));
        }

        public async Task<StandardResponse<(IEnumerable<TransactionResponseDto> transactions, MetaData metaData)>> GetAllUserTransaction(TransactionRequestParameters requestParameters, string userId, bool trackChanges)
        {
            PagedList<Transaction> transactions = await _unitOfWork.TransactionRepository
                .GetAllUserTransactionsAsync(requestParameters,userId, trackChanges);
            IEnumerable<TransactionResponseDto> responseDtos = _mapper.Map<IEnumerable<TransactionResponseDto>>(transactions);
            return StandardResponse<(IEnumerable<TransactionResponseDto> transactions, MetaData metaData)>
                .Success("Success", (responseDtos, transactions.MetaData));
        }

        public async Task<StandardResponse<TransactionResponseDto>> GetTransactionById(int id, bool trackChanges)
        {
            Transaction transaction = await GetTransactionWithId(id, trackChanges);
            TransactionResponseDto responseDto = _mapper.Map<TransactionResponseDto>(transaction);
            return StandardResponse<TransactionResponseDto>.Success("Success", responseDto);
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
    }
}
