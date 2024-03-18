using Microsoft.EntityFrameworkCore;
using Posterr.Core.Application;
using Posterr.Core.Domain.Boundaries.Configuration;
using Posterr.Infrastructure.Persistence;
using Posterr.Presentation.API.Controllers;
using Posterr.Presentation.API.EntryPoint;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new Exception("No database connnection string found");
builder.Services.ConfigurePersistence(connectionString);
builder.Services.ConfigureUseCases();
builder.Services.ConfigureControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDomainConfig, DomainConfig>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider
         .GetRequiredService<ApplicationDbContext>()
         .Database
         .Migrate();
}

app.Run();

public partial class Program
{
    // this is here only to allow integration tests to be able to reference the Program class
}
