using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Shared.Exceptions.Base
{
    public abstract class NotAlterable:Exception
    {
        public NotAlterable(string message):base(message)
        {
            
        }
    }
}
