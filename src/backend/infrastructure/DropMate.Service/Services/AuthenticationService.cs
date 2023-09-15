using AutoMapper;
using DropMate.Application.Common;
using DropMate.Application.ServiceContracts;
using DropMate.Domain.Models;
using DropMate.Shared.Dtos.Request;
using DropMate.Shared.Dtos.Response;
using DropMate.Shared.Exceptions.Sub;
using DropMate.Shared.HelperModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

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
            await CreateUserWallet(user.Id);
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
            await CreateUserWallet(user.Id);
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
            if(!(await _userManager.IsEmailConfirmedAsync(user)))
                throw new LoginFailedException("Email not yet confirm. Check your inbox");
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

        public void SendConfirmationEmail(string email, string callback_url)
        {
            string logoUrl = "https://res.cloudinary.com/djbkvjfxi/image/upload/v1694601350/uf4xfoda2c4z0exly8nx.png";
            string title = "DropMate Confirm Your Email";
            string body = $"<html><body><br/><br/>Please click to confirm your email address for DropMate Delivery. When you confirm your email you get full access to DropMate services for free.<p/> <a href={callback_url}>Verify Your Email</a> <p/><br/>DropMate is a game-changing delivery platform designed to simplify your life. Say goodbye to the hassles of traditional delivery services and experience a whole new level of convenience. Whether you need groceries, packages, or your favorite takeout, DropMate connects you with a network of reliable couriers who are ready to pick up and drop off your items with lightning speed. With real-time tracking, secure payments, and a seamless user interface, DropMate ensures that your deliveries are not only efficient but also stress-free. It's time to embrace a smarter way to send and receive goods – it's time for DropMate.<p/><br/><br/>With Love from the DropMate Team<p/>Thank you for choosing DropMate.<p/><img src={logoUrl}></body></html>";
            _emailService.SendEmail(email, title, body);
        }

        public void SendResetPasswordEmail(string email, string callback_url)
        {
            string logoUrl = "https://res.cloudinary.com/djbkvjfxi/image/upload/v1694601350/uf4xfoda2c4z0exly8nx.png";
            string title = "DropMate Reset Password";
            string body = $"<html><body><br/><br/>We hope this message finds you well. We wanted to inform you that a request to reset the password for your DropMate account was received. If you did not initiate this password reset, please disregard this email. Your account security is important to us, and we take all necessary precautions to protect it.<p/>Please click on the link to reset your password. <p/> <a href={callback_url}>Reset Your Password</a> <p/><p/>DropMate is a game-changing delivery platform designed to simplify your life. Say goodbye to the hassles of traditional delivery services and experience a whole new level of convenience. Whether you need groceries, packages, or your favorite takeout, DropMate connects you with a network of reliable couriers who are ready to pick up and drop off your items with lightning speed. With real-time tracking, secure payments, and a seamless user interface, DropMate ensures that your deliveries are not only efficient but also stress-free. It's time to embrace a smarter way to send and receive goods – it's time for DropMate.<p/><br/><br/>With Love from the DropMate Team<p/>Thank you for choosing DropMate.<p/><img src={logoUrl}></body></html>";
            _emailService.SendEmail(email, title, body);
        }
        private async Task CreateUserWallet(string userId)
        {
            var _paymentHelper = new PaymentHelper();
            var content = new StringContent(JsonSerializer.Serialize(new {Id =  userId}));
            using(HttpResponseMessage response =await _paymentHelper.ApiHelper.PostAsync("wallets", content))
            {
                if(!response.IsSuccessStatusCode)
                {
                    throw new RegisterErrorException("Wallet failed to create");
                }
            }
        }
    }
}
