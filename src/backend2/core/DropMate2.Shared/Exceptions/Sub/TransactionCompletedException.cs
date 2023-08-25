using DropMate2.Shared.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Shared.Exceptions.Sub
{
    public class TransactionCompletedException:NotAlterableException
    {
        public TransactionCompletedException(int id):base($"Transaction is already completed")
        {
            
        }
    }

}
