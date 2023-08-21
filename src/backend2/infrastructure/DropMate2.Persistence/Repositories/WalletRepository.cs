using DropMate2.Application.Contracts;
using DropMate2.Domain.Models;
using DropMate2.Persistence.Common;
using DropMate2.Shared.RequestFeatures;
using DropMate2.Shared.RequestFeatures.Common;
using Microsoft.EntityFrameworkCore;

namespace DropMate2.Persistence.Repositories
{
    public class WalletRepository :RepositoryBase<Wallet>, IWalletRepository
    {
        public WalletRepository(RepositoryContext context):base(context)
        {
            
        }
        public void CreateWallet(Wallet wallet)
        {
            Add(wallet);
        }

        public void DeleteWallet(Wallet wallet)
        {
            Delete(wallet);
        }

        public async Task<PagedList<Wallet>> GetAllWalletsAsync(WalletRequestParameter requestParameter, bool trackChanges)
        {
            List<Wallet> result = await FindAll(trackChanges)
                .Skip((requestParameter.PageNumber-1)*requestParameter.PageSize)
                .Take(requestParameter.PageSize).ToListAsync();

            int count = await FindAll(trackChanges).CountAsync();

            return new PagedList<Wallet>(result, count,requestParameter.PageNumber,requestParameter.PageSize);
        }

        public async Task<Wallet> GetWalletByIdAsync(int id, bool trackChanges)
        {
            return await FindByCondition(w => w.Id.Equals(id), trackChanges).FirstOrDefaultAsync();
        }

        public void PermanentDeleteMultiWallets(IEnumerable<Wallet> wallets)
        {
            PermanentDeleteRange(wallets);
        }

        public void PermanentWalletPackage(Wallet wallet)
        {
            PermanentDelete(wallet);
        }

        public void UpdateWallet(Wallet wallet)
        {
            Update(wallet);
        }
    }
}
