using DropMate2.Application.Common;
using DropMate2.LoggerService;
using DropMate2.Persistence.Common;
using DropMate2.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureUnitOfWork();
builder.Services.ConfigureLoggerManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigurePayStackHelper();
builder.Services.AddAutoMapper(typeof(Program));

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
