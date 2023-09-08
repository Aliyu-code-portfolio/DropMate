using AutoMapper;
using DropMate.Application.Common;
using DropMate.Application.ServiceContracts;
using DropMate.Domain.Models;
using DropMate.Shared.Dtos.Request;
using DropMate.Shared.Dtos.Response;
using DropMate.Shared.Exceptions.Sub;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DropMate.Service.Services
{
    internal sealed class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AuthenticationService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager,IEmailService emailService, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _emailService = emailService;
            _configuration = configuration;
        }
        public async Task<string> RegisterUser(UserCreateRequestDto requestDto)
        {
            User user = _mapper.Map<User>(requestDto);
            user.UserName = requestDto.Email;
            IdentityResult result = await _userManager.CreateAsync(user, requestDto.Password);
            if (!result.Succeeded)
            {
                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    if (!error.Description.Contains("Username"))
                        errors += error.Description.TrimEnd('.') + ", ";
                }
                throw new RegisterErrorException(errors.TrimEnd(',',' '));
            }
            _userManager.AddToRoleAsync(user, "User");
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }
        public async Task<string> RegisterAdmin(UserCreateRequestDto requestDto)
        {
            User user = _mapper.Map<User>(requestDto);
            user.UserName = requestDto.Email;
            IdentityResult result = await _userManager.CreateAsync(user, requestDto.Password);
            if (!result.Succeeded)
            {
                string errors = string.Empty;
                foreach(var error in result.Errors)
                {
                    if(!error.Description.Contains("Username"))
                        errors +=  error.Description.TrimEnd('.') + ", ";
                }
                throw new RegisterErrorException(errors.TrimEnd(',', ' '));
            }
            _userManager.AddToRoleAsync(user, "Admin");
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<StandardResponse<(string, UserResponseDto)>> ValidateAndCreateToken(UserLoginDto requestDto)
        {
            User user = await _userManager.FindByEmailAsync(requestDto.Email);
            bool result = await _userManager.CheckPasswordAsync(user, requestDto.Password);
            if (!result)
            {
                throw new LoginFailedException();
            }
            string token = await CreateToken(user);
            UserResponseDto userDto = _mapper.Map<UserResponseDto>(user);
            return StandardResponse<(string, UserResponseDto)>.Success("Successful", (token, userDto));
        }

        public async Task<string> GenerateEmailActivationToken(string email)
        {
            User user = await _userManager.FindByEmailAsync(email)
                 ?? throw new UserNotFoundException(email);
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public void SendEmailToken(string email, string title, string message)
        {
            _emailService.SendEmail(email, title, message);
        }

        private async Task<string> CreateToken(User user)
        {
            SigningCredentials signingCredentials = GetServerSigningCredentials();
            List<Claim> claims = await GetClaims(user);
            JwtSecurityToken tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }


        private SigningCredentials GetServerSigningCredentials()
        {
            string envSecret = Environment.GetEnvironmentVariable("SECRET");
            byte[] key = Encoding.UTF8.GetBytes(envSecret);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        private async Task<List<Claim>> GetClaims(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                 new Claim(ClaimTypes.Name,user.UserName),
                 new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            IList<string> roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            IConfigurationSection jwtSettings = _configuration.GetSection("JwtSettings");
            JwtSecurityToken tokenOptions = new JwtSecurityToken
                (
                    issuer: jwtSettings["validIssuer"],
                    audience: jwtSettings["validAudience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
                    signingCredentials: signingCredentials
                );
            return tokenOptions;
        }

        public async Task ConfirmEmailAddress(string email, string token)
        {
            string trimedToken = token.Replace(" ", "+");
            User user = await _userManager.FindByEmailAsync(email)
                ?? throw new UserNotFoundException(email);
            if(user.EmailConfirmed)
            {
                throw new EmailConfirmationException(user.Email);
            }
            IdentityResult result = await _userManager.ConfirmEmailAsync(user, trimedToken);
            if (!result.Succeeded)
            {
                throw new EmailConfirmationException(user.Email);
            }
        }

        public async Task ResetPassword(string token, UserLoginDto requestDto)
        {
            string trimedToken = token.Replace(" ", "+");
            User user = await _userManager.FindByEmailAsync(requestDto.Email)
                ?? throw new UserNotFoundException(requestDto.Email);
            IdentityResult result = await _userManager.ResetPasswordAsync(user, trimedToken, requestDto.Password);
            if (!result.Succeeded)
            {
                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += error.Description.TrimEnd('.') + ", ";
                }
                throw new PasswordResetFailedException(errors.TrimEnd(',', ' '));
            }
        }

        public async Task<string> GeneratePasswordResetToken(string email)
        {
            User user = await _userManager.FindByEmailAsync(email)
                ?? throw new UserNotFoundException(email);
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task ChangePassword(string email, ChangePasswordRequestDto requestDto)
        {
            User user = await _userManager.FindByEmailAsync(email)
                ?? throw new UserNotFoundException(email);
            IdentityResult result = await _userManager.ChangePasswordAsync(user, requestDto.OldPassword, requestDto.NewPassword);
            if (!result.Succeeded)
            {
                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += error.Description.TrimEnd('.') + ", ";
                }
                throw new PasswordChangeFailedException(errors.TrimEnd(',', ' '));
            }
        }
    }
}
