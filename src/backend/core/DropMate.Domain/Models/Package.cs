using DropMate.Domain.Common;
using DropMate.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DropMate.Domain.Models
{
    public class Package:BaseEntity
    {
        [ForeignKey(nameof(TravelPlan))]
        public int TravelPlanId { get; set; }

        [Required(ErrorMessage ="User ID is required")]
        [ForeignKey(nameof(User))]
        public string PackageOwnerId { get; set; }

        [Required(ErrorMessage = "Delivery contact name is required.")]
        [MaxLength(50, ErrorMessage = "Maximum length is 50")]
        public string DeliveryContactName { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        public string DeliveryContactNumber { get; set; }

        [MaxLength(200,ErrorMessage ="Maximum length is 200")]
        public string PackageImageUrl { get; set; }

        [Required(ErrorMessage = "Departure location is required.")]
        public LagosLocation DepartureLocation { get; set; }

        [Required(ErrorMessage = "Arrival location is required.")]
        public LagosLocation ArrivalLocation { get; set; }

        [Required(ErrorMessage = "Departure date and time is required.")]
        public DateTime DepartureDateTime { get; set; }

        [Required(ErrorMessage = "Package weight is required.")]
        public PackageWeight PackageWeight { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public Status Status { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation properties
        public TravelPlan TravelPlan { get; set; }
        public User Owner { get; set; }
        public Review Review { get; set; }

    }
}
