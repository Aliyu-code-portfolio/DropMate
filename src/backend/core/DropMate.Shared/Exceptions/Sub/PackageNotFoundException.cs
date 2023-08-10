using DropMate.Shared.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Shared.Exceptions.Sub
{
    public class PackageNotFoundException:NotFoundException
    {
        public PackageNotFoundException(object id):base($"The Package with identity: {id} is not found")
        {
            
        }
    }
}
