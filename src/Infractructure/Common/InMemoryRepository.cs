using Domain;

namespace Infractructure.Common;

public abstract class InMemoryRepository<T,TKey>
    where T: IEntity<TKey>
    where TKey: notnull
{
    private Dictionary<TKey, T> _storage = new Dictionary<TKey, T>();
    protected Task<T?> GetById(TKey id, CancellationToken cancellationToken = default)
    {
        _storage.TryGetValue(id, out var result);
        return Task.FromResult(result);
    }

    protected Task<IEnumerable<T>> Find(Func<T, bool> condition, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_storage.Values.Where(condition).AsEnumerable());
    }

    protected Task<T> Create(T entity, CancellationToken cancellationToken = default)
    {
        _storage.Add(entity.Id, entity);
        return Task.FromResult(entity);
    }
    protected Task<T> Update(T entity, CancellationToken cancellationToken = default)
    {
        _storage[entity.Id] = entity;
        return Task.FromResult(entity);
    }
    protected Task<T> Delete(T entity, CancellationToken cancellationToken = default)
    {
        _storage.Remove(entity.Id);
        return Task.FromResult(entity);
    }
    protected Task Delete(TKey id, CancellationToken cancellationToken = default)
    {
        _storage.Remove(id);
        return Task.CompletedTask;
    }
}
