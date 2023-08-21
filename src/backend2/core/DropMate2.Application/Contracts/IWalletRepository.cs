using DropMate2.Domain.Models;
using DropMate2.Shared.RequestFeatures;
using DropMate2.Shared.RequestFeatures.Common;

namespace DropMate2.Application.Contracts
{
    public interface IWalletRepository
    {
        Task<PagedList<Wallet>> GetAllWalletsAsync(WalletRequestParameter requestParameter, bool trackChanges);
        Task<Wallet> GetWalletByIdAsync(int id, bool trackChanges);
        void UpdateWallet(Wallet wallet);
        void DeleteWallet(Wallet wallet);
        void PermanentWalletPackage(Wallet wallet);
        void PermanentDeleteMultiWallets(IEnumerable<Wallet> wallets);
        void CreateWallet(Wallet wallet);
    }
}
