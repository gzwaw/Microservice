using Microsoft.EntityFrameworkCore;
using MilleniumTest.Interfaces;
using MilleniumTest.Handlers;
using MilleniumTest.Data;


var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

builder.Services.AddScoped<IAccountHandler, AccountHandler>();

builder.Services.AddDbContext<TestDbContext>(options =>
    options.UseInMemoryDatabase("AccountsDb"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

//Jeœli chcemy autoryzowaæ u¿ytkownika
//app.UseAuthorization();

//Jeœli chcemy dodatkowo sprawdzaæ to¿samoœæ u¿ytkownika
//app.UseAuthentication();

app.MapControllers();

app.Run();