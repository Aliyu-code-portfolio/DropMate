using DropMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Shared.Dtos.Request
{
    public record ReviewRequestDto
    {
        [Required(ErrorMessage = "Package ID is required")]
        public int PackageId { get; init; }

        [Required(ErrorMessage = "Rate is required.")]
        public Rate Rate { get; init; }

        [MaxLength(500, ErrorMessage = "Comment can't exceed 500 characters.")]
        public string? Comment { get; init; }
    }
}
