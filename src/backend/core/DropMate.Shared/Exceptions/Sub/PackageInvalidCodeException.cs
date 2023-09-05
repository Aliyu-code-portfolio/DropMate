using DropMate.Shared.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Shared.Exceptions.Sub
{
    public class PackageInvalidCodeException:InvalidCodeException
    {
        public PackageInvalidCodeException(object id):base($"The package with identity: {id} update failed due to invalid code")
        {
            
        }
    }
}
