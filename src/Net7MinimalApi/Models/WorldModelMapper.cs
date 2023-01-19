using Domain.Aggregates.Worlds;

namespace Net7MinimalApi.Models
{
    public static class WorldModelMapper
    {
        public static WorldModel? MapToModel(this World? entity)
        {
            if (entity is null)
            {
                return null;
            }
            return new WorldModel { Id = entity.Id, Name = entity.Name };
        }
        public static World MapToEntity(this WorldModel model)
        {
            return new World { Id = model.Id.Value, Name = model.Name };
        }
    }
}
