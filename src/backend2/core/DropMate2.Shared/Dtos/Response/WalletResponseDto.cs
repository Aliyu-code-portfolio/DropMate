using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Shared.Dtos.Response
{
    public class WalletResponseDto
    {
        public string Id { get; init; }
        public decimal Balance { get; init; }
    }
}
