using Domain.Aggregates.Worlds;
using Infractructure.Common;

namespace Infractructure.Repositories;

public class WorldRepository : InMemoryRepository<World, int>, IWorldRepository
{
    public Task<IEnumerable<World>> GetAll(CancellationToken cancelationToken = default)
        => base.Find((x) => true, cancelationToken);
    public new Task<World?> GetById(int id, CancellationToken cancelationToken = default)
        => base.GetById(id, cancelationToken);
    public async Task<World?> GetByName(string name, CancellationToken cancelationToken = default)
        => (await base.Find((x) => x.Name == name, cancelationToken)).SingleOrDefault();
    public new Task<World> Create(World world, CancellationToken cancelationToken)
        => base.Create(world, cancelationToken);
    public new Task<World> Update(World world, CancellationToken cancelationToken)
        => base.Update(world, cancelationToken);
    public new Task<World> Delete(World world, CancellationToken cancelationToken)
        => base.Delete(world, cancelationToken);
    public new Task Delete(int id, CancellationToken cancelationToken = default)
        => base.Delete(id, cancelationToken);
}
