using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Shared.Exceptions.Sub
{
    public class PaymentNotMadeExceptioin:PaymentFailedException
    {
        public PaymentNotMadeExceptioin(string refcode):base($"Payment for {refcode} not made")
        {
            
        }
        public PaymentNotMadeExceptioin():base("Failed to verify payment... Check reference code")
        {
            
        }
    }
}
