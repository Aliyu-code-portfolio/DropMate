using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Shared.Dtos.Response
{
    public class TransactionResponseDto
    {
        public int Id { get; set; }
        public decimal PaymentAmount { get; set; }
        public bool IsCompleted { get; set; }
        public string RecieverWalletID { get; set; }
        public string SenderWalletID { get; set; }
        public int? TravelPlanId { get; set; }
        public int? PackageId { get; set; }
    }
}
