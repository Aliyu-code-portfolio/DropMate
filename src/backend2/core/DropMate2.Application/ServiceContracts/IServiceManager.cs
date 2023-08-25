using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Application.ServiceContracts
{
    public interface IServiceManager
    {
        IWalletService WalletService { get; }
        ITransactionService TransactionService { get; }
        IDepositService DepositService { get; }

    }
}
