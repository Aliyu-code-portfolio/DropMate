using DropMate.Application.ServiceContracts;
using DropMate.ControllerEndPoints.ValidationFilter;
using DropMate.Shared.Dtos.Request;
using DropMate.Shared.Dtos.Response;
using DropMate.Shared.RequestFeature;
using DropMate.Shared.RequestFeature.Common;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DropMate.ControllerEndPoints.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IServiceManager _services;

        public UsersController(IServiceManager services)
        {
            _services = services;
        }

        [HttpGet]
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

        //Remove when auth is implemented
        [HttpPost]
        [ServiceFilter(typeof(ValidationActionFilters))]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateRequestDto requestDto)
        {
            StandardResponse<UserResponseDto> user = await _services.UserService.CreateUser(requestDto);
            return CreatedAtAction(nameof(GetUsersById),new { Id= user.Data.Id}, user.Data);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationActionFilters))]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserUpdateRequestDto requestDto)
        {
            await _services.UserService.UpdateUser(id, requestDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await _services.UserService.DeleteUser(id, false);
            return Ok();
        }
    }
}
