using DropMate2.Domain.Common;

namespace DropMate2.Domain.Models
{
    public class InitializedPayment:BaseEntity
    {
        public string WalletId { get; set; }
        public string Authorization_url { get; set; }
        public decimal Amount { get; set; }
        public string Reference { get; set; }
        public bool IsDeleted { get; set;}
    }
}
