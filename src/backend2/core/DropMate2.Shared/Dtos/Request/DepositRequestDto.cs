using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Shared.Dtos.Request
{
    public class DepositRequestDto
    {
        [Required(ErrorMessage = "The wallet id is required")]
        public string WalletId { get; set; }
        [EmailAddress(ErrorMessage ="Email is invalid")]
        [Required(ErrorMessage ="Email address is required")]
        public string Email { get; set; }
        [Range(50,double.MaxValue)]
        public decimal Amount { get; set; }

    }
}
