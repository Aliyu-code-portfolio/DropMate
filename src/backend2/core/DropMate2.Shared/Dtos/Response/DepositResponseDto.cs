using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Shared.Dtos.Response
{
    public record DepositResponseDto
    {
        public int Id { get; init; }
        public DateTime CreatedDate { get; init; }
        public string Reference { get; init; }
        public string WalletId { get; init; }

        public decimal Amount { get; init; }
    }
}
