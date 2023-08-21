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
    public interface IDepositRepository
    {
        Task<PagedList<Deposit>> GetAllDepositsAsync(DepositRequestParameter requestParameter, bool trackChanges);
        Task<PagedList<Deposit>> GetAllWalletDepositsAsync(DepositRequestParameter requestParameter, string walletId, bool trackChanges);
        Task<Deposit> GetDepositeByIdAsync(int id, bool trackChanges);
        void UpdateDeposit(Deposit deposit);
        void DeleteDeposit(Deposit deposit);
        void PermanentDeleteDeposit(Deposit deposit);
        void PermanentDeleteMultiDeposits(IEnumerable<Deposit> deposits);
        void CreateDeposit(Deposit deposit);
    }
}
