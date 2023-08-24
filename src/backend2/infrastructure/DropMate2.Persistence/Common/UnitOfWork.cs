using DropMate2.Application.Common;
using DropMate2.Application.Contracts;
using DropMate2.Persistence.Repositories;

namespace DropMate2.Persistence.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly Lazy<IWalletRepository> _walletRepository;
        private readonly Lazy<IDepositRepository> _depositRepository;
        private readonly Lazy<ITransactionRepository> _transactionRepository;
        private readonly Lazy<IInitializedPaymentRepository> _initializedPaymentRepository;
        public UnitOfWork(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _walletRepository = new Lazy<IWalletRepository>(()=>new WalletRepository(repositoryContext));
            _depositRepository = new Lazy<IDepositRepository>(()=>new DepositRepository(repositoryContext));
            _transactionRepository = new Lazy<ITransactionRepository>(()=>new TransactionRepository(repositoryContext));
            _initializedPaymentRepository = new Lazy<IInitializedPaymentRepository>(()=>new InitializedPaymentRepository(repositoryContext));
        }

        public IWalletRepository WalletRepository => _walletRepository.Value;

        public IDepositRepository DepositRepository => _depositRepository.Value;

        public ITransactionRepository TransactionRepository => _transactionRepository.Value;

        public IInitializedPaymentRepository InitializedPaymentRepository => _initializedPaymentRepository.Value;

        public async Task SaveAsync()
        {
            await _repositoryContext.SaveChangesAsync();
        }
    }
}
