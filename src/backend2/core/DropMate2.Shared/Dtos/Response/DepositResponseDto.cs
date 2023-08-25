using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Shared.Dtos.Response
{
    public class DepositResponseDto
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Reference { get; set; }
        public string WalletId { get; set; }

        public decimal Amount { get; set; }
    }
}
