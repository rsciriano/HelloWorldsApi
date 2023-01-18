namespace Domain.Aggregates.Worlds;

public class World: IEntity<int>
{
    public int Id { get; set; }
    public string? Name { get; set; }
}
