using DropMate.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Persistence.Common
{
    public class RepositoryContext:DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options):base(options)
        {
            
        }

        DbSet<User> Users { get; set; }
        DbSet<TravelPlan> TravelPlans { get; set; }
        DbSet<Package> Packages { get; set; }
        DbSet<Review> Reviews { get; set; } 
    }
}
