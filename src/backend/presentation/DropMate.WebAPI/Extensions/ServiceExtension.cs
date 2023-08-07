using DropMate.Persistence.Common;
using Microsoft.EntityFrameworkCore;

namespace DropMate.WebAPI.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)=> 
            services.AddDbContext<RepositoryContext>(option=>option.UseSqlServer(configuration.GetConnectionString("Default")));
    }
}
