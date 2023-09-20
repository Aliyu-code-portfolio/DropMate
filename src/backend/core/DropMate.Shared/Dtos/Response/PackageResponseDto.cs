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
    public record PackageResponseDto
    {
        public int Id { get; init; }
        public string ProductName { get; init; }
        public DateTime CreatedDate { get; init; }
        public int? TravelPlanId { get; init; }
        
        public string PackageOwnerId { get; init; }

        public string DeliveryContactName { get; init; }

        public string DeliveryContactNumber { get; init; }

        public string? PackageImageUrl { get; init; }
 
        public string DepartureLocation { get; init; }

        public string ArrivalLocation { get; init; }

        public DateTime DepartureDateTime { get; init; }

        public string PackageWeight { get; init; }
        public decimal Price { get; init; }
        public string? EstimatedDuration { get; init; }
        public int RecieveCode { get; init; }
        public int DeliverCode { get; init; }

        public string Status { get; init; }
        //public TravelPlanResponse TravelPlan { get; set; }
    }
}
