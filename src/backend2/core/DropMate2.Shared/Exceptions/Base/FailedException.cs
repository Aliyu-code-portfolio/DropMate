using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Shared.Exceptions.Base
{
    public class FailedException:Exception
    {
        public FailedException(string message):base(message)
        {
            
        }
    }
}
