using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Shared.Dtos.Request.Payment
{
    public class InitializePaymentRequestDto
    {
        public string Email { get; set; }
        public decimal Amount { get; set; }
        public string Callback_url { get; set; }
    }
}
