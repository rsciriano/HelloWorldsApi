using Domain.Aggregates.Worlds;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Net7ClassicApi.Controllers;

[Route("api/worlds")]
public class WorldsController : Controller
{
    private readonly IWorldRepository _repository;

    public WorldsController(IWorldRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    [HttpGet(Name = "GetAllWorlds")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<World>))]
    [SwaggerOperation(Tags = new[] { "Worlds" })]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _repository.GetAll());
    }

    [HttpGet("{id:int}", Name = "GetWorldById")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(World))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Tags = new[] { "Worlds" })]
    public async Task<IActionResult> GetById(int id)
    {
        var world = await _repository.GetById(id);

        if (world == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(world);
        }
    }

    [HttpGet("{name:regex(^[[a-zA-Z_-]]+$)}", Name = "GetWorldByName")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(World))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Tags = new[] { "Worlds" })]
    public async Task<IActionResult> GetByName(string name)
    {
        var world = await _repository.GetByName(name);

        if (world == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(world);
        }
    }

    [HttpPost(Name = "CreateWorld")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(World))]
    [SwaggerOperation(Tags = new[] { "Worlds" })]
    public async Task<IActionResult> Create([FromBody]World world)
    {
        return CreatedAtRoute(
            "GetWorldById",
            new { id = world.Id },
            await _repository.Create(world));
    }

    [HttpPut(Name = "UpdateWorld")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(World))]
    [SwaggerOperation(Tags = new[] { "Worlds" })]
    public async Task<IActionResult> Update([FromBody] World world)
    {        
        return Ok(await _repository.Update(world));
    }

    [HttpDelete("{id:int}", Name = "DeleteWorld")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(World))]
    [SwaggerOperation(Tags = new[] { "Worlds" })]
    public async Task<IActionResult> Delete(int id)
    {
        await _repository.Delete(id);

        return NoContent();
    }
}
