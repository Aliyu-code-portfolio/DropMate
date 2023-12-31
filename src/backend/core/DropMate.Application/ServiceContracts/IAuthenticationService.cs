﻿using DropMate.Shared.Dtos.Request;
using DropMate.Shared.Dtos.Response;

namespace DropMate.Application.ServiceContracts
{
    public interface IAuthenticationService
    {
        Task<string> RegisterUser(UserCreateRequestDto requestDto);
        Task<string> RegisterAdmin(UserCreateRequestDto requestDto);
        Task<StandardResponse<(string, UserResponseDto)>> ValidateAndCreateToken(UserLoginDto requestDto);
        void SendConfirmationEmail(string email, string callback_url);
        void SendResetPasswordEmail(string email, string callback_url);
        Task ConfirmEmailAddress(string email, string token);
        Task ResetPassword(string token,UserLoginDto requestDto);
        Task ChangePassword(string email, ChangePasswordRequestDto requestDto);
        Task<string> GenerateEmailActivationToken(string email);
        Task<string> GeneratePasswordResetToken(string email);
        Task AddUserAsAdmin(string email);


        //Add OAuth2.0
    }
}
