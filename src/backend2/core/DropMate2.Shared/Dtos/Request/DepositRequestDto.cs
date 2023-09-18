using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Shared.Dtos.Request
{
    public record DepositRequestDto
    {
        [Range(50,double.MaxValue)]
        public decimal Amount { get; init; }

    }
}
