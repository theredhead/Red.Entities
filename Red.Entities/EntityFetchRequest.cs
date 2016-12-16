using System;
using System.Collections.Generic;

namespace Red.Entities
{
    public class EntityFetchRequest
    {

        public Dictionary<string, object> Parameters
        {
            get;
            private set;
        } = new Dictionary<string, object>();

        public EntityType EntityType
        {
            get;
            set;
        }

        public MatchRequirement Match
        {
            get;
            set;
        } = MatchRequirement.All;

        public EntityFetchPredicate CreatePredicate()
        {
            var predicate = new EntityFetchPredicate(this);
            Predicates.Add(predicate);
            return predicate;
        }

        public List<EntityFetchPredicate> Predicates
        {
            get;
            set;
        } = new List<EntityFetchPredicate>();

        public EntityFetchRequest(EntityType entityType)
        {
            EntityType = entityType;
        }
    }

}
