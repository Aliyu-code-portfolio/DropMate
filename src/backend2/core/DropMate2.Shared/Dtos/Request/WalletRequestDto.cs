using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Shared.Dtos.Request
{
    public class WalletRequestDto
    {
        [Required(ErrorMessage ="The wallet id is required")]
        public string Id { get; set; }
    }
}
