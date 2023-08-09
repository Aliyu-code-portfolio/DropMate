using DropMate.Domain.Enums;
using DropMate.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Shared.Dtos.Request
{
    public record PackageRequestDto
    {
        public int TravelPlanId { get; init; }

        //*Pull this info from the current logged in user
        [Required(ErrorMessage = "User ID is required")]
        public string PackageOwnerId { get; init; }

        [Required(ErrorMessage = "Delivery contact name is required.")]
        [MaxLength(50, ErrorMessage = "Maximum length is 50")]
        public string DeliveryContactName { get; init; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        public string DeliveryContactNumber { get; init; }

        //Save the image and save the url here
        [MaxLength(200, ErrorMessage = "Maximum length is 200")]
        public string PackageImageUrl { get; init; }

        [Required(ErrorMessage = "Departure location is required.")]
        public LagosLocation DepartureLocation { get; init; }

        [Required(ErrorMessage = "Arrival location is required.")]
        public LagosLocation ArrivalLocation { get; init; }

        [Required(ErrorMessage = "Departure date and time is required.")]
        public DateTime DepartureDateTime { get; init; }

        [Required(ErrorMessage = "Package weight is required.")]
        public PackageWeight PackageWeight { get; init; }

    }
}
