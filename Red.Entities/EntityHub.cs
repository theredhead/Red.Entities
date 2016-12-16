using System;
using System.Collections.Generic;
namespace Red.Entities
{
    /// <summary>
    /// An EntityHub provides a non sql n API to a specific database and tracks objects that are currently in use
    /// </summary>
    public class EntityHub
    {
        protected Dictionary<string, Entity> trackedEntities = new Dictionary<string, Entity>();
        protected Dictionary<string, EntityType> tables = new Dictionary<string, EntityType>();

        public string Name
        {
            get;
            private set;
        }

        public bool IsTracking(Entity entity)
        {
            return trackedEntities.ContainsKey(entity.GloballyUniqueIdentifier);
        }

        public void Track(Entity entity)
        {
            trackedEntities[entity.GloballyUniqueIdentifier] = entity;
        }

        public void StopTracking(Entity entity)
        {
            trackedEntities.Remove(entity.GloballyUniqueIdentifier);
        }

        public IEnumerable<Entity> Fetch(EntityFetchRequest request)
        {
            IEnumerable<Entity> fetched = request.EntityType.Fetch(request);
            foreach (Entity entity in fetched)
            {
                Track(entity);
            }
            return fetched;
        }
    }
}
