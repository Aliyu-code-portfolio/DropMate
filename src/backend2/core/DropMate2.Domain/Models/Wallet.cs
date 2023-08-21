using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DropMate2.Domain.Models
{
    public class Wallet
    {
        [Key]
        public string Id { get; set; }
        [Column(TypeName ="money")]
        public decimal Balance { get; set; }

        public virtual ICollection<Deposit>? Deposits { get; set; }
    }
}
