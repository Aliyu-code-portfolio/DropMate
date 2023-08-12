using DropMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Shared.Dtos.Response
{
    public record ReviewResponseDto
    {
        public int Id { get; init; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int TravelPlanId { get; init; }

        public Rate Rate { get; init; }

        public string? Comment { get; init; }
    }
}
