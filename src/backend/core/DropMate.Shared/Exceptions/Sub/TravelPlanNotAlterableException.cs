using DropMate.Shared.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Shared.Exceptions.Sub
{
    public class TravelPlanNotAlterableException:NotAlterable
    {
        public TravelPlanNotAlterableException(object id):base($"The Travel plan with identity: {id} is cannot be alter anymore in the database. The plan has been subscribed to")
        {
            
        }
    }
}
