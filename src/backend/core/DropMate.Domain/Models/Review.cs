using DropMate.Domain.Common;
using DropMate.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DropMate.Domain.Models
{
    public class Review:BaseEntity
    {
        [Required(ErrorMessage ="Package ID is required")]
        public int PackageId { get; set; }

        [Required(ErrorMessage = "Rate is required.")]
        public Rate Rate { get; set; }

        [MaxLength(500, ErrorMessage = "Comment can't exceed 500 characters.")]
        public string Comment { get; set; }

        // Navigation properties
        public Package Package { get; set; }
        public User User { get; set; }
    }
}
