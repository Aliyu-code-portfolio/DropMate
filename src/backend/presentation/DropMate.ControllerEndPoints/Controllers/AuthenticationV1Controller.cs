using DropMate.Application.ServiceContracts;
using DropMate.ControllerEndPoints.ValidationFilter;
using DropMate.Shared.Dtos.Request;
using DropMate.Shared.Dtos.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace DropMate.ControllerEndPoints.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/authentication")]
    public class AuthenticationV1Controller : ControllerBase
    {

        private readonly IServiceManager _services;

        public AuthenticationV1Controller(IServiceManager services)
        {
            _services = services;
        }

      
        [HttpPost("register")]
        [ServiceFilter(typeof(ValidationActionFilters))]
        public async Task<IActionResult> RegisterUser([FromForm] UserCreateRequestDto requestDto)
        {
            string token = await _services.AuthenticationService.RegisterUser(requestDto);

            string encodedToken = System.Text.Encodings.Web.UrlEncoder.Default.Encode(token);
            string callback_url = Request.Scheme + "://" + Request.Host + $"/api/authentication/confirm-email/{requestDto.Email}/{encodedToken}";

            _services.AuthenticationService.SendConfirmationEmail(requestDto.Email, callback_url);
            return StatusCode(201,StandardResponse<string>.Success("Account created successfully. Please confirm your email",null,201));
        }

        [HttpGet]
        public async Task<IActionResult> Test()
        {

            return Ok("Request sent");
        }
        
        [HttpPost("add-admin")]
        public async Task<IActionResult> AddAmin([FromForm] string email)
        {
            await _services.AuthenticationService.AddUserAsAdmin(email);
            return StatusCode(200,StandardResponse<string>.Success("Add admin successfully.",null,200));
        }
        

        
        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationActionFilters))]
        public async Task<IActionResult> Login([FromForm] UserLoginDto requestDto)
        {
            StandardResponse<(string token, UserResponseDto userData)> result = await _services.AuthenticationService.ValidateAndCreateToken(requestDto);
            return Ok(StandardResponse<object>.Success("Login successful", new { Token = result.Data.token }, 200));
        }
        
        
        [HttpGet("activate-email/{email}")]
        public async Task<IActionResult> ActivateEmail(string email)
        {
            string token = await _services.AuthenticationService.GenerateEmailActivationToken(email);

            string encodedToken = System.Text.Encodings.Web.UrlEncoder.Default.Encode(token);
            string callback_url = Request.Scheme + "://" + Request.Host + $"/api/authentication/confirm-email/{email}/{encodedToken}";

            _services.AuthenticationService.SendConfirmationEmail(email, callback_url);
            return StatusCode(200, StandardResponse<string>.Success("Email verification successfully sent. Please confirm your email", null, 200));

        }
        
        [HttpGet("confirm-email/{email}/{token}")]
        public async Task<ContentResult> ConfirmEmail(string email, string token)
        {
            string decodedToken = WebUtility.UrlDecode(token);
            await _services.AuthenticationService.ConfirmEmailAddress(email, decodedToken);
             string htmlContent = @"
                <!DOCTYPE html>
                    <html lang=""en"">
                    <head>
                        <meta charset=""UTF-8"">
                        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                        <title>Email Verified</title>
                        <style>
                            /* Center the verification container */
                            body {
                                display: flex;
                                justify-content: center;
                                align-items: center;
                                height: 100vh;
                                margin: 0;
                            }

                            /* Style for the white background with shadow */
                            .verification-container {
                                background-color: #ffffff;
                                padding: 20px;
                                text-align: center;
                                border-radius: 5px;
                                box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
                            }

                            /* Style for the checkmark icon */
                            .checkmark {
                                font-size: 48px;
                                color: #00cc00; /* Green color for the checkmark */
                            }

                            /* Style for the ""Email Verified"" text */
                            .verified-text {
                                font-size: 24px;
                                color: #333333;
                                margin-top: 10px;
                            }

                            /* Style for the ""Welcome to DropMate Delivery"" text */
                            .dropmate-text {
                                font-size: 28px;
                                color: #333333;
                                margin-top: 10px;
                                font-weight: bold;
                            }
                        </style>
                    </head>
                    <body>
                        <div class=""verification-container"">
                            <!-- Checkmark icon -->
                            <div class=""checkmark"">&#10003;</div>
        
                            <!-- ""Email Verified"" text -->
                            <div class=""verified-text"">Verified Successfully</div>
        
                            <!-- ""Welcome to DropMate Delivery"" text -->
                            <div class=""dropmate-text"">Welcome to DropMate Delivery</div>
                        </div>
                    </body>
                    </html>";
            return new ContentResult
            {
                Content = htmlContent,
                ContentType = "text/html"
            };
        }

        [HttpGet("forget-password/{email}")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            string resetToken = await _services.AuthenticationService.GeneratePasswordResetToken(email);

            string encodedToken = System.Text.Encodings.Web.UrlEncoder.Default.Encode(resetToken);

            //Change to call the frontend url for entering new password and resetting with this resetToken passed in the header
            string callback_url = Request.Scheme + "://" + Request.Host + $"/api/authentication/confirm-email/{email}/{encodedToken}";//currently backend url

            _services.AuthenticationService.SendResetPasswordEmail(email, callback_url);
            return StatusCode(200, StandardResponse<string>.Success("Password reset successfully sent to your email.", null, 200));

        }

        [HttpGet("reset-password/{token}")]
        public async Task<IActionResult> ResetPassword(string token, [FromBody] UserLoginDto requestDto)
        {
            string decodedToken = WebUtility.UrlDecode(token);

            await _services.AuthenticationService.ResetPassword(decodedToken, requestDto);
            return Ok(StandardResponse<string>.Success("Your password has been reset successfully", null, 200));
        }
        
        [HttpGet("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto requestDto)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userNameClaim = claimsIdentity.FindFirst(ClaimTypes.Name);
            string email = userNameClaim.Value;
            await _services.AuthenticationService.ChangePassword(email, requestDto);
            return Ok(StandardResponse<string>.Success("Your password has been changed successfully", null, 201));
        }
        //remove due to industrial standards
        [HttpPost("register/admin")]
        [ServiceFilter(typeof(ValidationActionFilters))]
        public async Task<IActionResult> RegisterAdmin([FromForm] UserCreateRequestDto requestDto)
        {
            string token = await _services.AuthenticationService.RegisterAdmin(requestDto);

            string encodedToken = System.Text.Encodings.Web.UrlEncoder.Default.Encode(token);
            string callback_url = Request.Scheme + "://" + Request.Host + $"/api/authentication/confirm-email/{requestDto.Email}/{encodedToken}";

            _services.AuthenticationService.SendConfirmationEmail(requestDto.Email, callback_url);
            return StatusCode(201, StandardResponse<string>.Success("Account created successfully. Please confirm your email", null, 201));
        }
    }
}
