using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Shared.Dtos.Request
{
    public record UserLoginDto
    {
        [EmailAddress(ErrorMessage ="Email address is invalid")]
        public string Email { get; init; }
        public string Password { get; init; }
    }
}
