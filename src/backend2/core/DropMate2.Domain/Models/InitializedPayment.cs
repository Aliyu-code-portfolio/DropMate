using DropMate2.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DropMate2.Domain.Models
{
    public class InitializedPayment:IEntityBase
    {
        [Key]
        public string Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }
        public string WalletId { get; set; }
        public string Authorization_url { get; set; }
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }
        public string Access_code { get; set; }
        public string Reference { get; set; }
    }
}
