using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Shared.Dtos.Response.Payment
{
    public class InitializePaymentResponseDto
    {
        public string Authorization_url { get; set; }
        public string Access_code { get; set; }
        public string Reference { get; set; }
    }
}
