using System.ComponentModel.DataAnnotations;

namespace DropMate2.Domain.Common
{
    public class BaseEntity:IEntityBase
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
