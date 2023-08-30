using DropMate.Application.ServiceContracts;
using DropMate.ControllerEndPoints.ValidationFilter;
using DropMate.Shared.Dtos.Request;
using DropMate.Shared.Dtos.Response;
using DropMate.Shared.RequestFeature;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
            await _services.AuthenticationService.RegisterUser(requestDto);
            return StatusCode(201);
        }
        
        [HttpPost("register/admin")]
        [ServiceFilter(typeof(ValidationActionFilters))]
        public async Task<IActionResult> RegisterAdmin([FromBody] UserCreateRequestDto requestDto)
        {
            await _services.AuthenticationService.RegisterUser(requestDto);
            return StatusCode(201);
        }
        
        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationActionFilters))]
        public async Task<IActionResult> Login([FromBody] UserLoginDto requestDto)
        {
            StandardResponse<(string token, UserResponseDto userData)> result = await _services.AuthenticationService.ValidateAndCreateToken(requestDto);
            return Ok(new {Token = result.Data.token, UserData = result.Data.userData});
        }

    }
}
