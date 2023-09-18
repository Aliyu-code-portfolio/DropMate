using DropMate2.Shared.Dtos.Request;
using DropMate2.Shared.Dtos.Response;
using DropMate2.Shared.Dtos.Response.Payment;
using DropMate2.Shared.RequestFeatures;
using DropMate2.Shared.RequestFeatures.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Application.ServiceContracts
{
    public interface IDepositService
    {
        Task<StandardResponse<(IEnumerable<DepositResponseDto> deposits, MetaData metaData)>> GetAllDeposit(DepositRequestParameter requestParameters, bool trackChanges);
        Task<StandardResponse<(IEnumerable<DepositResponseDto> deposits, MetaData metaData)>> GetAllWalletDeposit(DepositRequestParameter requestParameters, string wallerId, bool trackChanges);
        Task<StandardResponse<DepositResponseDto>> GetDepositById(int id, bool trackChanges);
        Task<StandardResponse<Data>> InitializeDeposit(DepositRequestDto transaction,string email, string userId);
        Task CompleteDeposit(string referenceCode);
        Task DeleteDeposit(int id);
    }
}
