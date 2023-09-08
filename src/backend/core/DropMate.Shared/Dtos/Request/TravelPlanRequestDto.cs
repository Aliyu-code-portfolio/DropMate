using DropMate.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace DropMate.Shared.Dtos.Request
{
    public record TravelPlanRequestDto
    {
        //*Pull this info from the current logged in user
        /*[Required(ErrorMessage = "User ID is required")]
        public string TravelerId { get; set; }*/

        [Required(ErrorMessage = "Departure location is required.")]
        public LagosLocation DepartureLocation { get; init; }

        [Required(ErrorMessage = "Arrival location is required.")]
        public LagosLocation ArrivalLocation { get; init; }

        [Required(ErrorMessage = "Departure date and time is required.")]
        public DateTime DepartureDateTime { get; init; }

        [Required(ErrorMessage = "Maximum package weight is required.")]
        public PackageWeight MaximumPackageWeight { get; init; }


        //Package owner suggests a price

        /*[Column(TypeName = "money")]
        public decimal Price { get; set; }*/
    }
}
