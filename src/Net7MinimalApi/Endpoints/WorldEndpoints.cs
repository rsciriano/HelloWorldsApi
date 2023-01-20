using System.Reflection;
using System.Xml.Linq;
using Domain.Aggregates.Worlds;
using Net7MinimalApi.Models;

namespace Net7MinimalApi.Endpoints;

public static class WorldEndpoints
{
    public static RouteGroupBuilder MapWorldEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("api/worlds", GetAllWorlds)
        .Produces<IEnumerable<World>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetAllWorlds")
        .WithTags("Worlds");


        group.MapGet("api/worlds/{id:int}", GetWorldById)
        .Produces<World>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetWorldById")
        .WithTags("Worlds");

        group.MapGet("api/worlds/{name:regex(^[a-zA-Z_-]+$)}", GetWorldByName)
        .Produces<World>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetWorldByName")
        .WithTags("Worlds");

        group.MapPost("api/worlds", CreateWorld)
        .Produces<World>(StatusCodes.Status201Created)
        .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest)
        .WithName("CreateWorld")
        .WithTags("Worlds");

        group.MapPut("api/worlds", UpdateWorld)
        .Produces<World>(StatusCodes.Status200OK)
        .WithName("UpdateWorld")
        .WithTags("Worlds");

        group.MapDelete("api/worlds/{id:int}", DeleteWorld)
        .Produces(StatusCodes.Status204NoContent)
        .WithName("DeleteWorld")
        .WithTags("Worlds");

        return group;
    }

    private static async Task<IResult> GetAllWorlds(IWorldRepository repository)
    {
        return Results.Ok(await repository.GetAll());
    }

    private static async Task<IResult> GetWorldById(int id, IWorldRepository repository)
    {
        var world = await repository.GetById(id);

        if (world is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(world);
    }

    private static async Task<IResult> GetWorldByName(string name, IWorldRepository repository)
    {
        var world = await repository.GetByName(name);

        if (world is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(world);
    }

    private static async Task<IResult> CreateWorld(WorldModel model, IWorldRepository repository)
    {
        await repository.Create(model.MapToEntity());

        return Results.CreatedAtRoute("GetWorldById", new { id = model.Id }, model);
    }

    private static async Task<IResult> UpdateWorld(World world, IWorldRepository repository)
    {
        await repository.Update(world);

        return Results.Ok(world);
    }

    private static async Task<IResult> DeleteWorld(int id, IWorldRepository repository)
    {
        await repository.Delete(id);

        return Results.NoContent();
    }
}
