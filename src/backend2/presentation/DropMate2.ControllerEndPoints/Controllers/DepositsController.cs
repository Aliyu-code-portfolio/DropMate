using DropMate2.Application.ServiceContracts;
using DropMate2.Shared.Dtos.Request;
using DropMate2.Shared.Dtos.Response;
using DropMate2.Shared.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace DropMate2.ControllerEndPoints.Controllers
{
    [Route("api/deposits")]
    [ApiController]
    [Authorize]
    public class DepositsController : ControllerBase
    {
        private readonly IServiceManager _services;

        public DepositsController(IServiceManager services)
        {
            _services = services;
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllDeposits([FromQuery]DepositRequestParameter requestParameter)
        {
            var result = await _services.DepositService.GetAllDeposit(requestParameter, false);
            Response.Headers.Add("X-Pagination",JsonSerializer.Serialize(result.Data.metaData));
            return Ok(StandardResponse<IEnumerable<DepositResponseDto>>.Success("Retrieved successfully", result.Data.deposits, 200));
        }
        
        [HttpGet("user/id")]
        public async Task<IActionResult> GetAllUserDeposits([FromQuery]DepositRequestParameter requestParameter)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = userIdClaim.Value;
            var result = await _services.DepositService.GetAllWalletDeposit(requestParameter,userId, false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.Data.metaData));
            return Ok(StandardResponse<IEnumerable<DepositResponseDto>>.Success("Retrieved successfully", result.Data.deposits, 200));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepositById(int id)
        {
            var result = await _services.DepositService.GetDepositById(id, false);
            return Ok(result);
        }
        
        [HttpGet("confirm/{reference}")]
        public async Task<ContentResult> ConfirmDeposit(string reference)
        {
            await _services.DepositService.CompleteDeposit(reference);
            string htmlContent = @"
                <!DOCTYPE html>
                    <html lang=""en"">
                    <head>
                        <meta charset=""UTF-8"">
                        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                        <title>Payment Successful</title>
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
        
                            <!-- ""Pay Verified"" text -->
                            <div class=""verified-text"">Payment Successful</div>
        
                            <!-- ""Thanks"" text -->
                            <div class=""dropmate-text"">Thanks for choosing DropMate</div>
                        </div>
                    </body>
                    </html>";
            return new ContentResult
            {
                Content = htmlContent,
                ContentType = "text/html"
            };
        }

        [HttpPost]
        public async Task<IActionResult> InitializeDeposit([FromBody] DepositRequestDto requestDto)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userEmailClaim = claimsIdentity.FindFirst(ClaimTypes.Name);
            string email = userEmailClaim.Value;
            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = userIdClaim.Value;
            var result = await _services.DepositService.InitializeDeposit(requestDto,email, userId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeposit(int id)
        {
            await _services.DepositService.DeleteDeposit(id);
            return Ok(StandardResponse<string>.Success("Deposit deleted successfully", null, 200));
        }
    }
}
