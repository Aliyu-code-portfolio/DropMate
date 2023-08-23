using DropMate2.Shared.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Shared.Exceptions.Sub
{
    public class InsufficientFundFailedException:FailedException
    {
        public InsufficientFundFailedException(int packageId):base($"Insufficient wallet balance to pay for package with id: {packageId}\nPlease fund your wallet")
        {
            
        }
    }
}
