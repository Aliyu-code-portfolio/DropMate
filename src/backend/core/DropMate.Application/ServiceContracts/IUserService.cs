using DropMate.Shared.Dtos.Request;
using DropMate.Shared.Dtos.Response;
using DropMate.Shared.RequestFeature;
using DropMate.Shared.RequestFeature.Common;
using Microsoft.AspNetCore.Http;

namespace DropMate.Application.ServiceContracts
{
    public interface IUserService
    {
        Task<StandardResponse<UserResponseDto>> GetUserById(string id, bool trackChanges);
        Task<StandardResponse<UserResponseDto>> GetUserByEmail(string email, bool trackChanges);
        Task<StandardResponse<(IEnumerable<UserResponseDto> users, MetaData metaData)>> GetAllUsers(UserRequestParameters requestParameter, bool trackChanges);
        Task<StandardResponse<UserResponseDto>> CreateUser(UserCreateRequestDto requestDto);
        Task<StandardResponse<string>> UploadProfileImg(string id, IFormFile file);
        Task RemoveProfileImg(string id);
        Task UpdateUser(string id, UserUpdateRequestDto requestDto);
        Task DeleteUser(string id, bool trackChanges);
    }
}
