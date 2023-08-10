using DropMate.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DropMate.Domain.Models
{
    public class User:IBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(50, ErrorMessage ="Maximum length is 50")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(50, ErrorMessage = "Maximum length is 50")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        public string? PhoneNumber { get; set; }

        [MaxLength(200, ErrorMessage = "Maximum length is 200")]
        public string? Address { get; set; }

        [MaxLength(200, ErrorMessage = "Maximum length is 200")]
        public string? ProfilePicURL { get; set; }

        [Required]
        public DateTime DateJoined { get; set; }= DateTime.Now;

        public bool IsDeleted { get; set; }

        //Navigation property
        public virtual ICollection<TravelPlan>? TravelPlans { get; set; }
        public virtual ICollection<Package>? Packages { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }

    }
}
