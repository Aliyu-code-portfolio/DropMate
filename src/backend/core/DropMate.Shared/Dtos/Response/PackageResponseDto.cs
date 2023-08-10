﻿using DropMate.Domain.Enums;
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
        public DateTime CreatedDate { get; init; } = DateTime.Now;
        public int TravelPlanId { get; init; }
        
        public string PackageOwnerId { get; init; }

        public string DeliveryContactName { get; init; }

        public string DeliveryContactNumber { get; init; }

        public string PackageImageUrl { get; init; }
 
        public LagosLocation DepartureLocation { get; init; }

        public LagosLocation ArrivalLocation { get; init; }

        public DateTime DepartureDateTime { get; init; }

        public PackageWeight PackageWeight { get; init; }

        public Status Status { get; init; }
    }
}