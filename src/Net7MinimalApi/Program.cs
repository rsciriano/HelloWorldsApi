using Domain.Aggregates.Worlds;
using FluentValidation;
using Infractructure.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Net7MinimalApi.Extensions;
using Net7MinimalApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IWorldRepository, WorldRepository>();
builder.Services.AddSingleton<IValidator<WorldModel>, WorldModelValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.MapGet("api/hello", (string? name) => $"Hello {name ?? "world"}!")
    .WithName("Hello")
    .WithTags("Hello");

app.MapGet("api/worlds", async (IWorldRepository repository) =>
{
    return Results.Ok(await repository.GetAll());
})
.Produces<IEnumerable<World>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.WithName("GetAllWorlds")
.WithTags("Worlds");


app.MapGet("api/worlds/{id:int}", async (int id, IWorldRepository repository) =>
{
    var world = await repository.GetById(id);

    if (world is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(world);
})
.Produces<World>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.WithName("GetWorldById")
.WithTags("Worlds");

app.MapGet("api/worlds/{name:regex(^[a-zA-Z_-]+$)}", async (string name, IWorldRepository repository) =>
{
    var world = await repository.GetByName(name);

    if (world is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(world);
})
.Produces<World>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.WithName("GetWorldByName")
.WithTags("Worlds");

app.MapPost("api/worlds", async (WorldModel model, IValidator<WorldModel> validator, IWorldRepository repository) =>
{
    var validationResult = validator.Validate(model);
    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult.Errors.AsValidationProblemDetails());
    }

    await repository.Create(model.MapToEntity());

    return Results.CreatedAtRoute("GetWorldById", new { id = model.Id }, model);
})
.Produces<World>(StatusCodes.Status201Created)
.WithName("CreateWorld")
.WithTags("Worlds");

app.MapPut("api/worlds", async (World world, IWorldRepository repository) =>
{
    await repository.Update(world);

    return Results.Ok(world);
})
.Produces<World>(StatusCodes.Status200OK)
.WithName("UpdateWorld")
.WithTags("Worlds");

app.MapDelete("api/worlds/{id:int}", async (int id, IWorldRepository repository) =>
{
    await repository.Delete(id);

    return Results.NoContent();
})
.Produces(StatusCodes.Status204NoContent)
.WithName("DeleteWorld")
.WithTags("Worlds");

app.Run();


public partial class Program
{
}
