using DropMate2.Application.Common;
using DropMate2.Application.ServiceContracts;
using DropMate2.LoggerService;
using DropMate2.Persistence.Common;
using DropMate2.Service.Manager;
using DropMate2.Shared.HelperModels;
using Microsoft.EntityFrameworkCore;

namespace DropMate2.WebAPI.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RepositoryContext>(option => option
            .UseSqlServer(configuration.GetConnectionString("Default")));
        }
        public static void ConfigureUnitOfWork(this IServiceCollection services)=> services.AddScoped<IUnitOfWork, UnitOfWork>();
        public static void ConfigureLoggerManager(this IServiceCollection services) => services.AddSingleton<ILoggerManager, LoggerManager>();
        public static void ConfigureServiceManager(this IServiceCollection services)=> services.AddScoped<IServiceManager,ServiceManager>();
        public static void ConfigurePayStackHelper(this IServiceCollection services) => services.AddScoped<PayStackHelper>();
    }
}
