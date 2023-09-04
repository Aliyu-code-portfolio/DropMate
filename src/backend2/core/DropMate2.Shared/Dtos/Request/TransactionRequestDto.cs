using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Shared.Dtos.Request
{
    public record TransactionRequestDto
    {
        [Required(ErrorMessage = "Amount is required")]
        public decimal PaymentAmount { get; init; }
        [Required(ErrorMessage = "Receiver wallet id is required")]
        public string RecieverWalletID { get; init; }
        [Required(ErrorMessage = "Sender wallet id is required")]
        public string SenderWalletID { get; init; }
        [Required(ErrorMessage = "Travel plan id is required")]
        public int TravelPlanId { get; init; }
        [Required(ErrorMessage = "Package id is required")]
        public int PackageId { get; init; }
    }
}
