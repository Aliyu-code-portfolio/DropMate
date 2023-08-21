using DropMate.Application.Common;
using DropMate2.Application.Contracts;
using DropMate2.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Persistence.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly Lazy<IWalletRepository> _walletRepository;
        private readonly Lazy<IDepositRepository> _depositRepository;
        private readonly Lazy<ITransactionRepository> _transactionRepository;
        public UnitOfWork(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _walletRepository = new Lazy<IWalletRepository>(()=>new WalletRepository(repositoryContext));
            _depositRepository = new Lazy<IDepositRepository>(()=>new DepositRepository(repositoryContext));
            _transactionRepository = new Lazy<ITransactionRepository>(()=>new TransactionRepository(repositoryContext));
        }

        public IWalletRepository WalletRepository => _walletRepository.Value;

        public IDepositRepository DepositRepository => _depositRepository.Value;

        public ITransactionRepository TransactionRepository => _transactionRepository.Value;

        public async Task SaveAsync()
        {
            await _repositoryContext.SaveChangesAsync();
        }
    }
}
