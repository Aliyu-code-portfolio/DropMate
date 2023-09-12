using DropMate.Domain.Enums;
using DropMate.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Shared.Dtos.Response
{
    public record TravelPlanResponse
    {
        public int Id { get; init; }
        public DateTime CreatedDate { get; init; } 
        public string TravelerId { get; init; }

        public LagosLocation DepartureLocation { get; init; }

        public LagosLocation ArrivalLocation { get; init; }

        public DateTime DepartureDateTime { get; init; }

        public PackageWeight MaximumPackageWeight { get; init; }

        public bool IsActive { get; init; }
        public string? EstimatedPickUpTime { get; set; }
        public string? DistanceFromPickUp { get; set; }
        public ICollection<Package> Packages { get; set; }
        public UserResponseDto Traveler { get; init; }
    }
}
