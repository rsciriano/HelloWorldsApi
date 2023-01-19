using Domain.Aggregates.Worlds;

namespace Net7ClassicApi.Models
{
    public static class WorldModelMapper
    {
        public static WorldModel? Map(this World? entity)
        {
            if (entity is null)
            {
                return null;
            }
            return new WorldModel { Id = entity.Id, Name = entity.Name };
        }
        public static World Map(this WorldModel model)
        {
            return new World { Id = model.Id.Value, Name = model.Name };
        }
    }
}
