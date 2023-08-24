using DropMate2.Shared.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Shared.Exceptions.Sub
{
    public class DepositBadRequestException:NotFoundException
    {
        public DepositBadRequestException(string refCode):base($"Completion of deposit failed because the reference code: {refCode} is not found in our database")
        {
            
        }
    }
}
