using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Shared.Dtos.Request
{
    public record LocationBroadCastDto
    {
        public int TravelPlanId { get; init; }
        public double Latitude { get; init; }
        public double Longitude { get; init; }
    }
}
