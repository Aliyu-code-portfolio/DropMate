using DropMate.Domain.Models;
using DropMate.Persistence.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Persistence.Common
{
    public class RepositoryContext:IdentityDbContext<User>
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RoleConfiguration());
        }
        DbSet<User> Users { get; set; }
        DbSet<TravelPlan> TravelPlans { get; set; }
        DbSet<Package> Packages { get; set; }
        DbSet<Review> Reviews { get; set; } 
    }
}
