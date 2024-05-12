using AspNetCoreRateLimit;
using DropMate.Application.Common;
using DropMate.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using NLog;

var builder = WebApplication.CreateBuilder(args);
//LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(),
//"/nlog.config"));


// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureLoggerManager();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureUnitOfWork();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureActionFilter();
builder.Services.ConfigurePhotoService();
builder.Services.ConfigureEmailService();
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureApiVersioning();
builder.Services.ConfigureCaching();
builder.Services.ConfigureHttpCacheHeaders();
builder.Services.ConfigureIdentity();
builder.Services.AddMemoryCache();
builder.Services.ConfigureRateLimitingOptions();
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.ConfigureSwaggerAuth();

builder.Services.AddControllers(
    config=>
    {
        config.RespectBrowserAcceptHeader = true;
        config.CacheProfiles.Add("20 minutes cache", new CacheProfile() { Duration = 1200 });
    })
    .AddXmlDataContractSerializerFormatters()
    .AddApplicationPart(typeof(DropMate.ControllerEndPoints.Assembly).Assembly);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(logger);

// Configure the HTTP request pipeline.
//app.UseSwaggerAuthorized();
app.UseSwagger();
app.UseSwaggerUI(c=> c.SwaggerEndpoint("/swagger/v1/swagger.json", "DropMate API v1"));


app.UseHttpsRedirection();
app.UseIpRateLimiting();
app.UseCors("CorsPolicy");

app.UseResponseCaching();
app.UseHttpCacheHeaders();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
