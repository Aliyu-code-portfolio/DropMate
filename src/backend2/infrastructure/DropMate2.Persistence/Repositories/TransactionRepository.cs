using DropMate2.Application.Contracts;
using DropMate2.Domain.Models;
using DropMate2.Persistence.Common;
using DropMate2.Shared.RequestFeatures;
using DropMate2.Shared.RequestFeatures.Common;
using Microsoft.EntityFrameworkCore;

namespace DropMate2.Persistence.Repositories
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(RepositoryContext context):base(context)
        {
            
        }
        public void CreateTransaction(Transaction transaction)
        {
            Add(transaction);
        }

        public void DeleteTransaction(Transaction transaction)
        {
            Delete(transaction);
        }

        public async Task<PagedList<Transaction>> GetAllTransactionsAsync(TransactionRequestParameters requestParameter, bool trackChanges)
        {
            List<Transaction> result = await FindAll(trackChanges).Where(t=>!t.IsDeleted)
                .Skip((requestParameter.PageNumber-1)*requestParameter.PageSize)
                .Take(requestParameter.PageSize).ToListAsync();
            int count = await FindAll(trackChanges).CountAsync();
            return new PagedList<Transaction>(result, count, requestParameter.PageNumber,requestParameter.PageSize);
        }

        public async Task<PagedList<Transaction>> GetAllUserTransactionsAsync(TransactionRequestParameters requestParameter, string userId, bool trackChanges)
        {
            List<Transaction> result = await FindByCondition(t=>t.SenderWalletID.Equals(userId)
            ||t.RecieverWalletID.Equals(userId), trackChanges).Where(t => !t.IsDeleted)
                .Skip((requestParameter.PageNumber - 1) * requestParameter.PageSize)
                .Take(requestParameter.PageSize).ToListAsync();
            int count = await FindByCondition(t => t.SenderWalletID.Equals(userId)
            || t.RecieverWalletID.Equals(userId), trackChanges).CountAsync();
            return new PagedList<Transaction>(result, count, requestParameter.PageNumber, requestParameter.PageSize);
        }

        public async Task<Transaction> GetTransactionByIdAsync(int id, bool trackChanges)
        {
            return await FindByCondition(t => t.Id.Equals(id) && !t.IsDeleted, trackChanges).FirstOrDefaultAsync();
        }

        public async Task<Transaction> GetTransactionByPackageIdAsync(int packageId, bool trackChanges)
        {
            return await FindByCondition(t => t.PackageId.Equals(packageId) && !t.IsDeleted, trackChanges).FirstOrDefaultAsync();
        }

        public void PermanentDeleteMultiTransactions(IEnumerable<Transaction> transactions)
        {
            PermanentDeleteRange(transactions);
        }

        public void PermanentDeleteTransaction(Transaction transaction)
        {
            PermanentDelete(transaction);
        }

        public void UpdateTransaction(Transaction transaction)
        {
            Update(transaction);
        }
    }
}
