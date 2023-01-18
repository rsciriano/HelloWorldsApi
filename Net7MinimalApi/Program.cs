using Domain.Aggregates.Worlds;
using Infractructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IWorldRepository, WorldRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.MapGet("api/hello", (string name) => $"Hello {name ?? "world"}!");

app.MapGet("api/worlds", (IWorldRepository repository) =>
{
    return repository.GetAll();
});

app.MapGet("api/worlds/{id:int}", (int id, IWorldRepository repository) =>
{
    return repository.GetById(id);
});

app.MapGet("api/worlds/{name:regex(^[a-zA-Z_-]+$)}", (string name, IWorldRepository repository) =>
{
    return repository.GetByName(name);
});

app.MapPost("api/worlds", (World world, IWorldRepository repository) =>
{
    return repository.Create(world);
});

app.MapPut("api/worlds", (World world, IWorldRepository repository) =>
{
    return repository.Update(world);
});

app.MapDelete("api/worlds", (int id, IWorldRepository repository) =>
{
    return repository.Delete(id);
});

app.Run();


public partial class Program
{
}
