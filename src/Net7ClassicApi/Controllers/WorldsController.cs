using Domain.Aggregates.Worlds;
using Microsoft.AspNetCore.Mvc;

namespace Net7ClassicApi.Controllers;

[Route("api/worlds")]
public class WorldsController : Controller
{
    private readonly IWorldRepository _repository;

    public WorldsController(IWorldRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    [HttpGet]
    public Task<IEnumerable<World>> GetAll()
    {
        return _repository.GetAll();
    }
    [HttpGet("{id:int}")]
    public Task<World?> GetById(int id)
    {
        return _repository.GetById(id);
    }
    [HttpGet("{name:regex(^[[a-zA-Z_-]]+$)}")]
    public Task<World?> GetByName(string name)
    {
        return _repository.GetByName(name);
    }
    [HttpPost]
    public Task<World> Create([FromBody]World world)
    {
        return _repository.Create(world);
    }
    [HttpPut]
    public Task<World> Update([FromBody] World world)
    {
        return _repository.Update(world);
    }
    [HttpDelete("{id:int}")]
    public Task Delete(int id)
    {
        return _repository.Delete(id);
    }
}
