using DropMate2.Shared.Dtos.Request;
using DropMate2.Shared.Dtos.Response;
using DropMate2.Shared.RequestFeatures;
using DropMate2.Shared.RequestFeatures.Common;

namespace DropMate2.Application.ServiceContracts
{
    public interface IWalletService
    {
        Task<StandardResponse<(IEnumerable<WalletResponseDto> wallets, MetaData metaData)>> GetAllWallets(WalletRequestParameter requestParameters, bool trackChanges);
        Task<StandardResponse<WalletResponseDto>> GetWalletById(string id, bool trackChanges);
        Task CreateWallet(WalletRequestDto walletDto);
        Task DeleteWallet(string id);
        Task UpdateWallet(WalletRequestDto walletDto);
    }
}
