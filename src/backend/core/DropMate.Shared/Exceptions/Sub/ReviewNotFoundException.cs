using DropMate.Shared.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Shared.Exceptions.Sub
{
    public class ReviewNotFoundException:NotFoundException
    {
        public ReviewNotFoundException(object id): base($"The review with identity: {id} is not found in the database")
        {
            
        }
    }
}
