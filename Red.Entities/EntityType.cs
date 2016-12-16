using System;
using System.Collections.Generic;

namespace Red.Entities
{
    /// <summary>
    /// Represents the Schema of a Table and an interface to the Entities (Records) stored in that Table
    /// </summary>
    public class EntityType
    {
        public EntityDatabase Database { get; private set; }

        public string TableName { get; private set; }

        public string[] FieldNames { get; private set; }

        public EntityType(EntityDatabase database, string tableName, string[] fieldNames)
        {
            Database = database;
            TableName = tableName;
            FieldNames = fieldNames;
        }

        private Type _instanceType = typeof(Entity);
        public Type InstanceType
        {
            get
            {
                return _instanceType;
            }
            set
            {
                AssertValidEntityType(value);
                _instanceType = value;
            }
        }

        void AssertValidEntityType(Type type)
        {
            if (!type.IsSubclassOf(typeof(Entity)))
            {
                throw new NotSupportedException("A subclass of Entity is expected.");
            }
        }

        public EntityFetchRequest CreateFetchRequest()
        {
            var request = new EntityFetchRequest(this);
            return request;
        }

        public IEnumerable<Entity> Fetch(EntityFetchRequest request)
        {
            return Database.Fetch(request);
        }

        public string Name
        {
            get;
        }

        public Entity CreateInstance()
        {
            Entity entity = (Entity)Activator.CreateInstance(_instanceType, new[] { this });
            return entity;
        }

        public void Save(Entity entity)
        {
            if (shouldInsertEntity(entity))
            {
                willInsertEntity(entity);
                Insert(entity);
                didInsertEntity(entity);
            }
            else if (shouldUpdateEntity(entity))
            {
                willUpdateEntity(entity);
                Update(entity);
                didUpdateEntity(entity);
            }
        }

        protected void Insert(Entity entity)
        {

        }

        protected void Update(Entity entity)
        {

        }

        protected virtual void willInsertEntity(Entity entity)
        {
        }

        protected virtual void didInsertEntity(Entity entity)
        {
        }

        protected virtual void willUpdateEntity(Entity entity)
        {
        }

        protected virtual void didUpdateEntity(Entity entity)
        {
        }

        protected virtual bool shouldInsertEntity(Entity entity)
        {
            return entity.IsNew;
        }

        protected virtual bool shouldUpdateEntity(Entity entity)
        {
            return !entity.IsNew && entity.IsModified;
        }
    }
}
