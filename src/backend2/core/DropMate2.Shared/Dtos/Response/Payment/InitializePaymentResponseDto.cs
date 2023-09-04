using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Shared.Dtos.Response.Payment
{
    public record InitializePaymentResponseDto
    {
        public Data data { get; init; }

    }
}
