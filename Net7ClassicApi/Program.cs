using Domain.Aggregates.Worlds;
using Infractructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IWorldRepository, WorldRepository>();

var app = builder.Build();
app.MapControllers();
app.Run();


public partial class Program
{
}
