using AutoMapper;
using DropMate2.Application.Common;
using DropMate2.Application.ServiceContracts;
using DropMate2.Service.Services;
using DropMate2.Shared.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Service.Manager
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IWalletService> _walletService;
        private readonly Lazy<ITransactionService> _transactionService;
        private readonly Lazy<IDepositService> _depositService;

        public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper, PayStackHelper payStackHelper)
        {
            _walletService = new Lazy<IWalletService>(() => new WalletService(unitOfWork, mapper));
            _transactionService = new Lazy<ITransactionService>(() => new TransactionService(unitOfWork, mapper));
            _depositService = new Lazy<IDepositService>(()=> new DepositService(unitOfWork, mapper, payStackHelper));
        }

        public IWalletService WalletService => _walletService.Value;

        public ITransactionService TransactionService => _transactionService.Value;

        public IDepositService DepositService => _depositService.Value;
    }
}
