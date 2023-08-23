using DropMate2.Shared.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Shared.Exceptions.Sub
{
    public class TransactionAlreadyExistException:NotAlterableException
    {
        public TransactionAlreadyExistException(int packageId):base($"The package with id: {packageId} has an incomplete transaction already")
        {
            
        }
    }
}
