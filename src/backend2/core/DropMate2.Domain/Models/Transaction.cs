using DropMate2.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Domain.Models
{
    public class Transaction:BaseEntity
    {
        public decimal PaymentAmount { get; set; }
        public string RecieverWalletID { get; set; }
        public string SenderWalletID { get; set; }
        public int TravelPlanId { get; set; }
        public int PackageId { get; set; }
    }
}
