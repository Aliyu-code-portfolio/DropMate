using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Shared.Dtos.Response
{
    public class UserResponseDto
    {
        public string Id { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }
       
        public string Email { get; init; }

        public string PhoneNumber { get; init; }

        public string Address { get; init; }

        public string ProfilePicURL { get; init; }

        public DateTime DateJoined { get; init; } = DateTime.Now;

    }
}
