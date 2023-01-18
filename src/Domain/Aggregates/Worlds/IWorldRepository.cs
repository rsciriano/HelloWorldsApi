namespace Domain.Aggregates.Worlds;

public interface IWorldRepository
{
    Task<IEnumerable<World>> GetAll(CancellationToken cancelationToken = default);
    Task<World?> GetById(int id, CancellationToken cancelationToken = default);
    Task<World?> GetByName(string name, CancellationToken cancelationToken = default);
    Task<World> Create(World world, CancellationToken cancelationToken = default);
    Task<World> Update(World world, CancellationToken cancelationToken = default);
    Task<World> Delete(World world, CancellationToken cancelationToken = default);
    Task Delete(int id, CancellationToken cancelationToken = default);
}
