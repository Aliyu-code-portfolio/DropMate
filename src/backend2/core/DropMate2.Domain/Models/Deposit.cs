﻿using DropMate2.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DropMate2.Domain.Models
{
    public class Deposit:BaseEntity
    {
        [Required(ErrorMessage ="The wallet id is required")]
        [ForeignKey(nameof(Wallet))]
        public string WalletId { get; set; }

        public decimal Amount { get; set; }

        public virtual Wallet? Wallet { get; set; }
    }
}