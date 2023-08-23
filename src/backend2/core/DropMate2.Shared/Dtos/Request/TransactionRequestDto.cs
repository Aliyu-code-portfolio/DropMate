using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Shared.Dtos.Request
{
    public class TransactionRequestDto
    {
        [Required(ErrorMessage = "Amount is required")]
        public decimal PaymentAmount { get; set; }
        [Required(ErrorMessage = "Receiver wallet id is required")]
        public string RecieverWalletID { get; set; }
        [Required(ErrorMessage = "Sender wallet id is required")]
        public string SenderWalletID { get; set; }
        public int TravelPlanId { get; set; }
        public int PackageId { get; set; }
    }
}
