using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Red.Entities
{
    /// <summary>
    /// Represents a single Record of an EntityType (in a Table) and a basic interface to manipulating its data
    /// 
    /// Intended to be subclassed for each specific Table where the subclass should only need to add strongly 
    /// typed properties for the data fields used.
    /// </summary>
    public class Entity
    {
        private const string IDENTITY_FIELD_NAME = "_Id";
        private bool _isLoaded = false;

        public bool IsNew
        {
            get
            {
                return !_isLoaded;
            }
        }
        public bool IsModified
        {
            get
            {
                return ModifiedValues.Keys.Count > 0;
            }
        }

        /// <summary>
        /// Gets the globally unique identifier that can be used accross the wire to refer to this specific instance..
        /// </summary>
        /// <value>The globally unique identifier.</value>
        public string GloballyUniqueIdentifier
        {
            get
            {
                return $"{this.EntityType.Name}-{Identity}";
            }
        }

        public EntityType EntityType
        {
            get;
            private set;
        }

        #region Data storage
        /// <summary>
        /// Gets or sets the stored values.
        /// </summary>
        /// <value>The stored values.</value>
        private Dictionary<string, object> StoredValues
        {
            get;
            set;
        } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the modified values.
        /// </summary>
        /// <value>The modified values.</value>
        private Dictionary<string, object> ModifiedValues
        {
            get;
            set;
        } = new Dictionary<string, object>();

        /// <summary>
        /// Gets the data for all fields in their current unsaved state
        /// </summary>
        /// <value>The unsaved values.</value>
        private Dictionary<string, object> UnsavedValues
        {
            get
            {
                var unsaved = new Dictionary<string, object>();
                foreach (string key in StoredValues.Keys)
                {
                    unsaved[key] = this[key];
                }
                return unsaved;
            }
        }
        #endregion Data storage
        #region Sanity validation
        protected void AssertValidPropertyName(string propertyName)
        {
            if (!EntityType.FieldNames.Contains(propertyName))
            {
                throw new KeyNotFoundException($"{propertyName} does not exist in this {this.GetType().Name}");
            }
        }

        protected void AssertLoaded()
        {
            if (this.StoredValues.Keys.Count != EntityType.FieldNames.Length)
            {
                throw new InvalidOperationException("Operation is not valid before the instance is loaded.");
            }
        }

        protected void AssertHasEntityType()
        {
            if (this.EntityType == null)
            {
                throw new InvalidOperationException("Operation is not valid before the instance is attached to an EntityType.");
            }
        }
        #endregion Sanity validation
        #region Field access
        protected object this[string propertyName]
        {
            get
            {
                AssertLoaded();
                AssertValidPropertyName(propertyName);

                if (ModifiedValues.ContainsKey(propertyName))
                {
                    return ModifiedValues[propertyName];
                }
                else return StoredValues[propertyName];
            }
            set
            {
                AssertLoaded();
                AssertValidPropertyName(propertyName);

                if (ModifiedValues.ContainsKey(propertyName))
                {
                    if (ModifiedValues[propertyName] == value)
                    {
                        PropertyWillChange(propertyName);
                        ModifiedValues.Remove(propertyName);
                        PropertyDidChange(propertyName);
                    }
                    else
                    {
                        PropertyWillChange(propertyName);
                        ModifiedValues[propertyName] = value;
                        PropertyDidChange(propertyName);
                    }
                }
                else
                {
                    PropertyWillChange(propertyName);
                    ModifiedValues[propertyName] = value;
                    PropertyDidChange(propertyName);
                }
            }
        }
        #endregion Field access

        public virtual void PropertyWillChange(string propertyName)
        {
        }

        public virtual void PropertyDidChange(string propertyName)
        {
        }

        public long Identity
        {
            get { return (long)this[IDENTITY_FIELD_NAME]; }
            set { this[IDENTITY_FIELD_NAME] = value; }
        }

        public Entity(EntityType entityType)
        {
            EntityType = entityType;
        }

        public void Load(IDataReader reader)
        {
            foreach (string key in EntityType.FieldNames)
            {
                StoredValues[key] = reader[reader.GetOrdinal(key)];
            }
        }

        public void Save()
        {
            AssertHasEntityType();

            if (shouldSave())
            {
                willSave();
                EntityType.Save(this);
                didSave();
            }
        }

        protected virtual bool shouldSave()
        {
            return IsNew || IsModified;
        }

        protected virtual void willSave()
        {
        }

        protected virtual void didSave()
        {
        }
    }
}
