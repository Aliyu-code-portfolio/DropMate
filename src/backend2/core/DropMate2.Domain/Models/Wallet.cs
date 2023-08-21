using DropMate2.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DropMate2.Domain.Models
{
    public class Wallet:IEntityBase
    {
        [Key]
        public string Id { get; set; }
        [Column(TypeName ="money")]
        public decimal Balance { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Deposit>? Deposits { get; set; }
    }
}
