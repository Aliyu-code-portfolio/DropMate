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
        Task<string> RegisterUser(UserCreateRequestDto requestDto);
        Task<string> RegisterAdmin(UserCreateRequestDto requestDto);
        Task<StandardResponse<(string, UserResponseDto)>> ValidateAndCreateToken(UserLoginDto requestDto);
        void SendEmailToken(string email, string title, string message);
        Task ConfirmEmailAddress(string email, string token);
        Task ResetPassword(string token,UserLoginDto requestDto);
        Task ChangePassword(string email, ChangePasswordRequestDto requestDto);
        Task<string> GenerateEmailActivationToken(string email);
        Task<string> GeneratePasswordResetToken(string email);


        //Add OAuth2.0
    }
}
