using DropMate2.Domain.Models;
using DropMate2.Shared.RequestFeatures;
using DropMate2.Shared.RequestFeatures.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Application.Contracts
{
    public interface ITransactionRepository
    {
        Task<PagedList<Transaction>> GetAllTransactionsAsync(TransactionRequestParameters requestParameter, bool trackChanges);
        Task<PagedList<Transaction>> GetAllUserTransactionsAsync(TransactionRequestParameters requestParameter, string userId, bool trackChanges);
        Task<Transaction> GetTransactionByIdAsync(int id, bool trackChanges);
        void UpdateTransaction(Transaction transaction);
        void DeleteTransaction(Transaction transaction);
        void PermanentDeleteTransaction(Transaction transaction);
        void PermanentDeleteMultiTransactions(IEnumerable<Transaction> transactions);
        void CreateTransaction(Transaction transaction);
    }
}
