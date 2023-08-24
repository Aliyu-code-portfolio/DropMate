using DropMate2.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Persistence.Common
{
    public class RepositoryContext:DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options):base(options)
        {
        }
        DbSet<Wallet> Wallets { get; set; }
        DbSet<Deposit> Deposits { get; set; }
        DbSet<Transaction> Transactions { get; set; }
        DbSet<InitializedPayment> InitializedPayments { get; set; }
    }
}
