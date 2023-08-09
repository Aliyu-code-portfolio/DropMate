using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Shared.Exceptions.Base
{
    public abstract class NotFoundException:Exception
    {
        public NotFoundException(string message):base(message)
        {
            
        }
    }
}
