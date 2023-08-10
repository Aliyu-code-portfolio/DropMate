using DropMate.Shared.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Shared.Exceptions.Sub
{
    public class PackageNotAlterableException:NotAlterableException
    {
        public PackageNotAlterableException(object id):base($"The package with identity: {id} cannot be alter anymore in the database. The package has been booked")
        {
            
        }
    }
}
