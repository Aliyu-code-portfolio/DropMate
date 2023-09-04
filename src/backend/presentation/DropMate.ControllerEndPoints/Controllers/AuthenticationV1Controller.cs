using DropMate.Application.ServiceContracts;
using DropMate.ControllerEndPoints.ValidationFilter;
using DropMate.Shared.Dtos.Request;
using DropMate.Shared.Dtos.Response;
using DropMate.Shared.RequestFeature;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.ControllerEndPoints.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationV1Controller : ControllerBase
    {

        private readonly IServiceManager _services;

        public AuthenticationV1Controller(IServiceManager services)
        {
            _services = services;
        }

      
        [HttpPost("register")]
        [ServiceFilter(typeof(ValidationActionFilters))]
        public async Task<IActionResult> RegisterUser([FromBody] UserCreateRequestDto requestDto)
        {
            string token = await _services.AuthenticationService.RegisterUser(requestDto);

            string encodedToken = System.Text.Encodings.Web.UrlEncoder.Default.Encode(token);
            string callback_url = Request.Scheme + "://" + Request.Host + $"/api/authentication/confirm-email/{requestDto.Email}/{encodedToken}";

            _services.AuthenticationService.SendEmailToken(requestDto.Email,"Confirm Your Email for DropMate", $"Please click on the link to confirm your email address. " + callback_url);
            return StatusCode(201,"Account created successfully. Please confirm your email");
        }
        
        [HttpPost("register/admin")]
        [ServiceFilter(typeof(ValidationActionFilters))]
        public async Task<IActionResult> RegisterAdmin([FromBody] UserCreateRequestDto requestDto)
        {
            string token = await _services.AuthenticationService.RegisterAdmin(requestDto);

            string encodedToken = System.Text.Encodings.Web.UrlEncoder.Default.Encode(token);
            string callback_url = Request.Scheme + "://" + Request.Host + $"/api/authentication/confirm-email/{requestDto.Email}/{encodedToken}";

            _services.AuthenticationService.SendEmailToken(requestDto.Email, "Confirm Your Email for DropMate", $"Please click on the link to confirm your email address {requestDto.Email}. "+callback_url);
            return StatusCode(201, "Account created successfully. Please confirm your email");
        }
        
        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationActionFilters))]
        public async Task<IActionResult> Login([FromBody] UserLoginDto requestDto)
        {
            StandardResponse<(string token, UserResponseDto userData)> result = await _services.AuthenticationService.ValidateAndCreateToken(requestDto);
            return Ok(new {Token = result.Data.token, profile = result.Data.userData});
        }
        
        
        [HttpGet("activate-email/{email}")]
        public async Task<IActionResult> ActivateEmail(string email)
        {
            string token = await _services.AuthenticationService.GenerateEmailActivationToken(email);

            string encodedToken = System.Text.Encodings.Web.UrlEncoder.Default.Encode(token);
            string callback_url = Request.Scheme + "://" + Request.Host + $"/api/authentication/confirm-email/{email}/{encodedToken}";

            _services.AuthenticationService.SendEmailToken(email, "Confirm Your Email for DropMate", $"Please click on the link to confirm your email address {email}. " + callback_url);
            return StatusCode(200, "Email verification successfully sent. Please confirm your email");

        }
        
        [HttpGet("confirm-email/{email}/{token}")]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            string decodedToken = WebUtility.UrlDecode(token);
            await _services.AuthenticationService.ConfirmEmailAddress(email, decodedToken);
            //Redirect to your frontend
            return Ok("Your email has been confirmed successfully");
        }

        [HttpGet("forget-password/{email}")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            string resetToken = await _services.AuthenticationService.GeneratePasswordResetToken(email);

            string encodedToken = System.Text.Encodings.Web.UrlEncoder.Default.Encode(resetToken);

            //Change to call the frontend url for entering new password and resetting with this resetToken passed in the header
            string callback_url = Request.Scheme + "://" + Request.Host + $"/api/authentication/confirm-email/{email}/{encodedToken}";//currently backend url

            _services.AuthenticationService.SendEmailToken(email, "Reset Password for DropMate", $"Please click on the link to reset your password for {email}. " + callback_url);
            return StatusCode(200, "Password reset successfully sent to your email.");

        }

        [HttpGet("reset-password/{token}")]
        public async Task<IActionResult> ResetPassword(string token, [FromBody] UserLoginDto requestDto)
        {
            string decodedToken = WebUtility.UrlDecode(token);

            await _services.AuthenticationService.ResetPassword(decodedToken, requestDto);
            return Ok("Your password has been reset successfully");
        }
        
        [HttpGet("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto requestDto)
        {
            await _services.AuthenticationService.ChangePassword( requestDto);
            return Ok("Your password has been changed successfully");
        }

    }
}
