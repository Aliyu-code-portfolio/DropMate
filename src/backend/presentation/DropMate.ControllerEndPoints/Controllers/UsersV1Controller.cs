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
using System.Text.Json;

namespace DropMate.ControllerEndPoints.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/users")]
    [ApiController]
    public class UsersV1Controller : ControllerBase
    {

        private readonly IServiceManager _services;
        private readonly IEmailService emailService;

        public UsersV1Controller(IServiceManager services, IEmailService emailService)
        {
            _services = services;
            this.emailService = emailService;
        }

        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetAllUsers([FromQuery] UserRequestParameters requestParameter)
        {
            StandardResponse<(IEnumerable<UserResponseDto> users,MetaData metaData)> result = await _services.UserService.GetAllUsers(requestParameter,false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.Data.metaData));
            return Ok(result.Data.users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsersById(string id)
        {
            StandardResponse<UserResponseDto> user = await _services.UserService.GetUserById(id, false);
            return Ok(user);
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUsersByEmail(string email)
        {
            StandardResponse<UserResponseDto> user = await _services.UserService.GetUserByEmail(email, false);
            return Ok(user);
        }

        [HttpPost("{id}/profile-img")]
        public async Task<IActionResult> UploadProfileImg(string id, IFormFile file)
        {
            StandardResponse<string> result = await _services.UserService.UploadProfileImg(id, file);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationActionFilters))]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserUpdateRequestDto requestDto)
        {
            await _services.UserService.UpdateUser(id, requestDto);
            return Ok();
        }

        [HttpDelete("{id}/profile-img")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await _services.UserService.RemoveProfileImg(id);
            return Ok();
        }
        
        [HttpDelete("{id}/")]
        public async Task<IActionResult> DeleteProfileImg(string id)
        {
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
