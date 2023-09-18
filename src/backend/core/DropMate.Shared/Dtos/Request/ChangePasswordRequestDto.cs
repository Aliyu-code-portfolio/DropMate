using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Shared.Dtos.Request
{
    public record ChangePasswordRequestDto
    {
        public string OldPassword { get; init; }
        public string NewPassword { get; init; }
    }
}
