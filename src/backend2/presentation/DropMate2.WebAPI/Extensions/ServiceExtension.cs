using DropMate2.Application.Common;
using DropMate2.Application.ServiceContracts;
using DropMate2.LoggerService;
using DropMate2.Persistence.Common;
using DropMate2.Service.Manager;
using DropMate2.Shared.HelperModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace DropMate2.WebAPI.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureCors(this IServiceCollection services) =>
         services.AddCors(options =>
         {
             options.AddPolicy("CorsPolicy", builder =>
             builder.AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader());
         });
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RepositoryContext>(option => option
            .UseSqlServer(configuration.GetConnectionString("Default")));
        }
        public static void ConfigureUnitOfWork(this IServiceCollection services)=> services.AddScoped<IUnitOfWork, UnitOfWork>();
        public static void ConfigureLoggerManager(this IServiceCollection services) => services.AddSingleton<ILoggerManager, LoggerManager>();
        public static void ConfigureServiceManager(this IServiceCollection services)=> services.AddScoped<IServiceManager,ServiceManager>();
        public static void ConfigurePayStackHelper(this IServiceCollection services) => services.AddSingleton<PayStackHelper>();
        public static void ConfigureIdentityService(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings"); // Customize this to your configuration structure

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = jwtSettings["Authority"]; // URL of Microservice 1's identity provider
                    options.Audience = jwtSettings["Audience"]; // Audience for Microservice 2
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["ValidIssuer"], // Microservice 1's URL
                        ValidAudience = jwtSettings["Audience"], // Microservice 2's identifier
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"))) // Microservice 2's secret key
                    };
                });
        }
        public static void ConfigureSwaggerAuth(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Contacts API", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter an access token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
            });
        }
    }
}
