using DropMate.Application.ServiceContracts;
using DropMate.ControllerEndPoints.ValidationFilter;
using DropMate.Shared.Dtos.Request;
using DropMate.Shared.Dtos.Response;
using DropMate.Shared.RequestFeature;
using DropMate.Shared.RequestFeature.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text.Json;

namespace DropMate.ControllerEndPoints.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/users")]
    public class UsersV1Controller : ControllerBase
    {

        private readonly IServiceManager _services;

        public UsersV1Controller(IServiceManager services, IEmailService emailService)
        {
            _services = services;
        }

        [Authorize(Roles ="Admin")]
        [HttpGet]
        [HttpHead]
        [ResponseCache(CacheProfileName= "20 minutes cache")]
        public async Task<IActionResult> GetAllUsers([FromQuery] UserRequestParameters requestParameter)
        {
            StandardResponse<(IEnumerable<UserResponseDto> users,MetaData metaData)> result = await _services.UserService.GetAllUsers(requestParameter,false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.Data.metaData));
            return Ok(result.Data.users);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetUsersById()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userIdClaim  = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string id = userIdClaim.Value;
            StandardResponse<UserResponseDto> user = await _services.UserService.GetUserById(id, false);
            return Ok(user);
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUsersByEmail(string email)
        {
            StandardResponse<UserResponseDto> user = await _services.UserService.GetUserByEmail(email, false);
            return Ok(user);
        }

        [HttpPost("id/profile-img")]
        public async Task<IActionResult> UploadProfileImg(IFormFile file)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string id = userIdClaim.Value;
            StandardResponse<string> result = await _services.UserService.UploadProfileImg(id, file);
            return Ok(new { imgUrl = result });
        }

        [HttpPut("id")]
        [ServiceFilter(typeof(ValidationActionFilters))]
        public async Task<IActionResult> UpdateUser([FromForm] UserUpdateRequestDto requestDto)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string id = userIdClaim.Value;
            await _services.UserService.UpdateUser(id, requestDto);
            return Ok();
        }

        [HttpDelete("id/profile-img")]
        public async Task<IActionResult> DeleteUser()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string id = userIdClaim.Value;
            await _services.UserService.DeleteUser(id, false);
            return Ok();
        }
        
        [HttpDelete("id")]
        public async Task<IActionResult> DeleteProfileImg()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string id = userIdClaim.Value;
            await _services.UserService.RemoveProfileImg(id);
            return Ok();
        }
        [HttpOptions]
        public IActionResult Options()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST, PUT");
            return Ok();
        }

    }
}
