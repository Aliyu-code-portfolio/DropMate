using DropMate2.Application.Contracts;
using DropMate2.Domain.Models;
using DropMate2.Persistence.Common;
using DropMate2.Shared.RequestFeatures;
using DropMate2.Shared.RequestFeatures.Common;
using Microsoft.EntityFrameworkCore;

namespace DropMate2.Persistence.Repositories
{
    public class DepositRepository : RepositoryBase<Deposit>, IDepositRepository
    {
        public DepositRepository(RepositoryContext context):base(context)
        {
            
        }
        public void CreateDeposit(Deposit deposit)
        {
            Add(deposit);
        }

        public void DeleteDeposit(Deposit deposit)
        {
            Delete(deposit);
        }

        public async Task<PagedList<Deposit>> GetAllDepositsAsync(DepositRequestParameter requestParameter, bool trackChanges)
        {
            List<Deposit> result = await FindAll(trackChanges)
                .Skip((requestParameter.PageNumber - 1) * requestParameter.PageSize)
                .Take(requestParameter.PageSize).ToListAsync();
            int count = await FindAll(trackChanges).CountAsync();
            return new PagedList<Deposit>(result, count, requestParameter.PageNumber, requestParameter.PageSize);
        }

        public async Task<PagedList<Deposit>> GetAllWalletDepositsAsync(DepositRequestParameter requestParameter, string walletId, bool trackChanges)
        {
            List<Deposit> result = await FindByCondition(d=>d.WalletId.Equals(walletId), trackChanges)
                .Skip((requestParameter.PageNumber - 1) * requestParameter.PageSize)
                .Take(requestParameter.PageSize).ToListAsync();
            int count = await FindByCondition(d => d.WalletId.Equals(walletId), trackChanges).CountAsync();
            return new PagedList<Deposit>(result, count, requestParameter.PageNumber, requestParameter.PageSize);
        }

        public async Task<Deposit> GetDepositeByIdAsync(int id, bool trackChanges)
        {
           return await FindByCondition(d => d.Id.Equals(id), trackChanges).FirstOrDefaultAsync();
        }

        public void PermanentDeleteDeposit(Deposit deposit)
        {
            PermanentDelete(deposit);
        }

        public void PermanentDeleteMultiDeposits(IEnumerable<Deposit> deposits)
        {
            PermanentDeleteRange(deposits);
        }

        public void UpdateDeposit(Deposit deposit)
        {
            Update(deposit);
        }
    }
}
