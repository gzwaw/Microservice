using Application.Accounts.Commands.CreateAccount;
using Application.Common.Behaviours;
using Application.Mappings;
using FluentValidation;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebAPI;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

builder.Services.AddControllers();

builder.Services.AddDbContext<ServiceDbContext>(opt => opt.UseInMemoryDatabase("AccountsDb"));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AccountMapping>());

//Rejestracja validatorów
builder.Services.AddValidatorsFromAssemblyContaining<CreateAccountValidator>();

// Pipeline behavior – integracja FluentValidation z MediatR
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

//Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

//Jeœli chcemy autoryzowaæ u¿ytkownika
//app.UseAuthorization();

//Jeœli chcemy dodatkowo sprawdzaæ to¿samoœæ u¿ytkownika
//app.UseAuthentication();

app.MapControllers();

app.Run();