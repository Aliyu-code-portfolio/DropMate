using DropMate.Shared.Dtos.Request;
using DropMate.Shared.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Application.ServiceContracts
{
    public interface IUserService
    {
        Task<StandardResponse<UserResponseDto>> GetUserById(string id, bool trackChanges);
        Task<StandardResponse<UserResponseDto>> GetUserByEmail(string email, bool trackChanges);
        Task<StandardResponse<IEnumerable<UserResponseDto>>> GetAllUsers(bool trackChanges);
        Task<StandardResponse<UserResponseDto>> CreateUser(UserCreateRequestDto requestDto);
        Task UpdateUser(string id, UserUpdateRequestDto requestDto);
        Task DeleteUser(string id, bool trackChanges);
    }
}
