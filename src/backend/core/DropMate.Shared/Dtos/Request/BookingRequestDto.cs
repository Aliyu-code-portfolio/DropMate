using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Shared.Dtos.Request
{
    public record BookingRequestDto
    {
        [Required(ErrorMessage ="Travel Plan Id is required")]
        public int travelPlanId { get; set; }
        [Required(ErrorMessage = "Package Plan Id is required")]
        public int packageId { get; set; }
    }
}
