using System.ComponentModel.DataAnnotations;

namespace DropMate.Domain.Common
{
    public abstract class BaseEntity:IBaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }= DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
        [MaxLength(50)]
        public string? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
