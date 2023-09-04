using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Shared.Dtos.Response
{
    public record TransactionResponseDto
    {
        public int Id { get; init; }
        public decimal PaymentAmount { get; init; }
        public bool IsCompleted { get; init; }
        public string RecieverWalletID { get; init; }
        public string SenderWalletID { get; init; }
        public int? TravelPlanId { get; init; }
        public int? PackageId { get; init; }
    }
}
