using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Shared.Dtos.Request
{
    public record UserUpdateRequestDto
    {
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string? PhoneNumber { get; init; }

        [Required(ErrorMessage = "Address is required")]
        [MaxLength(200, ErrorMessage = "Maximum length is 200")]
        public string Address { get; init; }
    }
}
