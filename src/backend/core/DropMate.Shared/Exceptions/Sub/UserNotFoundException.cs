using DropMate.Shared.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Shared.Exceptions.Sub
{
    public class UserNotFoundException:NotFoundException
    {
        public UserNotFoundException(object id):base($"The user with identity: {id} is not found in the database")
        {
        }
    }
}
