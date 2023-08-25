using DropMate2.Shared.Dtos.Request;
using DropMate2.Shared.Dtos.Response;
using DropMate2.Shared.RequestFeatures;
using DropMate2.Shared.RequestFeatures.Common;

namespace DropMate2.Application.ServiceContracts
{
    public interface ITransactionService
    {
        Task<StandardResponse<(IEnumerable<TransactionResponseDto> transactions, MetaData metaData)>> GetAllTransaction(TransactionRequestParameters requestParameters, bool trackChanges);
        Task<StandardResponse<(IEnumerable<TransactionResponseDto> transactions, MetaData metaData)>> GetAllUserTransaction(TransactionRequestParameters requestParameters,string userId, bool trackChanges);
        Task<StandardResponse<TransactionResponseDto>> GetTransactionById(int id, bool trackChanges);
        Task<StandardResponse<TransactionResponseDto>> CreateTransaction(TransactionRequestDto transaction);
        Task CompleteTransaction(int packageId, bool isCompleted);
        Task DeleteTransaction(int id);
        Task UpdateTransaction(int id, TransactionRequestDto transaction);
    }
}
