using DropMate.Domain.Common;
using DropMate.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DropMate.Domain.Models
{
    public class TravelPlan:BaseEntity
    {
        [Required(ErrorMessage ="User ID is required")]
        [ForeignKey(nameof(User))]
        public string TravelerId { get; set; }

        [Required(ErrorMessage = "Departure location is required.")]
        public LagosLocation  DepartureLocation { get; set; }

        [Required(ErrorMessage = "Arrival location is required.")]
        public LagosLocation ArrivalLocation { get; set; }

        [Required(ErrorMessage = "Departure date and time is required.")]
        public DateTime DepartureDateTime { get; set; }

        [Required(ErrorMessage = "Maximum package weight is required.")]
        public PackageWeight MaximumPackageWeight { get; set; }

        public bool IsActive { get; set; } = true;
        public Status IsCompleted { get; set; }

        // Navigation property
        public virtual User? Traveler { get; set; }
        public virtual ICollection<Package>? Packages { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
