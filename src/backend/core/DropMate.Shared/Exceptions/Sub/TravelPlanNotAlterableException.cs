using DropMate.Shared.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Shared.Exceptions.Sub
{
    public class TravelPlanNotAlterableException:NotAlterableException
    {
        public TravelPlanNotAlterableException(int id):base($"The Travel plan with identity: {id} cannot be alter anymore in the database. The plan has been subscribed to")
        {
            
        }
        public TravelPlanNotAlterableException(string message):base(message)
        {
            
        }
    }
}
