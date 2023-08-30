using DropMate.Shared.Dtos.Request;
using DropMate.Shared.Dtos.Response;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Application.ServiceContracts
{
    public interface IAuthenticationService
    {
        Task RegisterUser(UserCreateRequestDto requestDto);
        Task RegisterAdmin(UserCreateRequestDto requestDto);
        Task<StandardResponse<(string, UserResponseDto)>> ValidateAndCreateToken(UserLoginDto requestDto);
        //Add OAuth2.0
    }
}
