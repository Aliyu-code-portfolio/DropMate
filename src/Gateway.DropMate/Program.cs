using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

//Add services here
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();
app.UseOcelot();
app.MapGet("/gateway", () => "You probably shouldn't be here but welcome to DropMate");

app.Run();
