using DropMate.Shared.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Shared.Exceptions.Sub
{
    public class MaxJoinDateBadRequest:BadRequestException
    {
        public MaxJoinDateBadRequest():base("User MinJoinDate cannot be greater than MaxJoinDate")
        {
            
        }
    }
}
