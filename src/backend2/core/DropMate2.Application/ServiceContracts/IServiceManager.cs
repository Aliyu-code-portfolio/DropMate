using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Application.ServiceContracts
{
    public interface IServiceManager
    {
        IWalletService IWalletService { get; }
        ITransactionService TransactionService { get; }
        IDepositService IDepositService { get; }

    }
}
