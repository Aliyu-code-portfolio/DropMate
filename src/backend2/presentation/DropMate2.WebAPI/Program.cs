using DropMate2.Application.Common;
using DropMate2.Service.Services;
using DropMate2.WebAPI.Extensions;
using NLog;

var builder = WebApplication.CreateBuilder(args);
//LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(),
//"/nlog.config"));

// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureUnitOfWork();
builder.Services.ConfigureLoggerManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureEmailService();
builder.Services.ConfigurePayStackHelper();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddSignalR();
builder.Services.ConfigureIdentityService(builder.Configuration);
builder.Services.ConfigureSwaggerAuth();

builder.Services.AddControllers(config => config.RespectBrowserAcceptHeader = true)
    .AddXmlDataContractSerializerFormatters()
    .AddApplicationPart(typeof(DropMate2.ControllerEndPoints.Assembly).Assembly);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(logger);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseRouting();

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseCors("ReactPolicy");
app.UseAuthorization();
/*app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<LocationTracker>("/location"); // Map the SignalR hub to a specific path
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=tracking}/{action=update}");
});*/
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<LocationTracker>("/tracking");
});         

app.MapControllers();

app.Run();
