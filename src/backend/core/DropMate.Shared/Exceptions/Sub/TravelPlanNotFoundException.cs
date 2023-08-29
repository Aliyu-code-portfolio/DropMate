using DropMate.Shared.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Shared.Exceptions.Sub
{
    public class TravelPlanNotFoundException:NotFoundException
    {
        public TravelPlanNotFoundException(int id):base($"The Travel Plan with identity: {id} is not found in the database")
        {
            
        }
    }
}
