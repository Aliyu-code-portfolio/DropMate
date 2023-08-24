using DropMate2.Application.Contracts;

namespace DropMate2.Application.Common
{
    public interface IUnitOfWork
    {
        IWalletRepository WalletRepository { get; }
        IDepositRepository DepositRepository { get; }
        ITransactionRepository TransactionRepository { get; }
        IInitializedPaymentRepository InitializedPaymentRepository { get; }
        Task SaveAsync();
    }
}
