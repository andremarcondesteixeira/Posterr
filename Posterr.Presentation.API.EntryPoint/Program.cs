using Posterr.Presentation.API.Controllers;
using Posterr.Core.Application;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureUseCases();
builder.Services.ConfigureControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program
{
    // this is here only to allow integration tests to be able to reference the Program class
}
