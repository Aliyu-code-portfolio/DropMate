using DropMate.Application.Common;
using DropMate.Application.ServiceContracts;
using DropMate.ControllerEndPoints.ValidationFilter;
using DropMate.Persistence.Common;
using DropMate.Service.Manager;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DropMate.WebAPI.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)=> 
            services.AddDbContext<RepositoryContext>(option=>option.UseSqlServer(configuration.GetConnectionString("Default")));

        public static void ConfigureLoggerManager(this IServiceCollection services) => services.AddSingleton<ILoggerManager, LoggerManager>();
        public static void ConfigureUnitOfWork(this IServiceCollection services)=> services.AddScoped<IUnitOfWork, UnitOfWork>();
        public static void ConfigureServiceManager(this IServiceCollection services) => services.AddScoped<IServiceManager, ServiceManager>();
        public static void ConfigureActionFilter(this IServiceCollection services) => services.AddScoped<ValidationActionFilters>();
        public static void ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
            });
        }
    }
}
