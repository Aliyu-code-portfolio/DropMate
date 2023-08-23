using DropMate2.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace DropMate2.Domain.Models
{
    public class Transaction:BaseEntity
    {
        [Required(ErrorMessage ="Amount is required")]
        public decimal PaymentAmount { get; set; }
        [Required(ErrorMessage = "Receiver wallet id is required")]
        public string RecieverWalletID { get; set; }
        [Required(ErrorMessage = "Sender wallet id is required")]
        public string SenderWalletID { get; set; }
        public bool IsCompleted { get; set; }
        public int TravelPlanId { get; set; }
        public int PackageId { get; set; }
    }
}
