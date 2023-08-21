using DropMate2.Application.Contracts;

namespace DropMate.Application.Common
{
    public interface IUnitOfWork
    {
        IWalletRepository WalletRepository { get; }
        IDepositRepository DepositRepository { get; }
        ITransactionRepository TransactionRepository { get; }
        Task SaveAsync();
    }
}
