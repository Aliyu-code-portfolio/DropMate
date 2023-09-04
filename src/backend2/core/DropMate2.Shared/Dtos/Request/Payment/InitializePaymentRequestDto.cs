using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Shared.Dtos.Request.Payment
{
    public record InitializePaymentRequestDto
    {
        public string email { get; init; }
        public string amount { get; init; }
        public string callback_url { get; init; }
    }
}
