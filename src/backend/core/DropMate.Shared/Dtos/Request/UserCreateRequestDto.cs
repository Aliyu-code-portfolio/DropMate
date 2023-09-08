using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Shared.Dtos.Request
{
    public record UserCreateRequestDto
    {

        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(50, ErrorMessage = "Maximum length is 50")]
        public string FirstName { get; init; }

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(50, ErrorMessage = "Maximum length is 50")]
        public string LastName { get; init; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; init; }

        [Required(ErrorMessage ="Password is required")]
        [PasswordPropertyText]
        public string Password { get; init; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        public string? PhoneNumber { get; init; }

        [Required(ErrorMessage ="Address is required")]
        [MaxLength(200, ErrorMessage = "Maximum length is 200")]
        public string Address { get; init; }

       /* create endpoint to upload profile pictures to cloudinary and save the url
        * 
        * [MaxLength(200, ErrorMessage = "Maximum length is 200")]
        public string? ProfilePicURL { get; set; }*/

    }
}
