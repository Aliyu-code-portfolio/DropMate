using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Shared.Dtos.Request.Payment
{
    public class InitializePaymentRequestDto
    {
        public string email { get; set; }
        public string amount { get; set; }
        public string callback_url { get; set; }
    }
}
