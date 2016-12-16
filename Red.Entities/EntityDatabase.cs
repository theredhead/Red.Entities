using System;
using System.Collections.Generic;
using System.Data;

namespace Red.Entities
{
    public class EntityDatabase
    {
        protected Dictionary<string, EntityType> Tables = new Dictionary<string, EntityType>();

        public EntityStatementBuilder StatementBuilder { get; private set; }

        public IDbConnection Connection { get; private set; }

        public EntityType this[string tableName]
        {
            get
            {
                return Tables[tableName];
            }
        }

        public EntityDatabase(IDbConnection connection, EntityStatementBuilder statementBuilder)
        {
            Connection = connection;
            StatementBuilder = statementBuilder;
        }

        private IDbCommand CreateSelectCommand(EntityFetchRequest request)
        {
            IDbCommand command = Connection.CreateCommand();
            command.CommandText = StatementBuilder.BuildSelectCommandText(request);
            foreach (var p in request.Parameters)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = StatementBuilder.FormatParameterName(p.Key);
                parameter.Value = p.Value != null ? p.Value : DBNull.Value;
                command.Parameters.Add(parameter);
            }

            return command;
        }

        public IEnumerable<Entity> Fetch(EntityFetchRequest request)
        {
            var result = new List<Entity>();

            using (IDbCommand command = CreateSelectCommand(request))
            {
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Entity entity = request.EntityType.CreateInstance();
                        entity.Load(reader);
                        result.Add(entity);
                    }
                }
            }
            return result;
        }

        public IEnumerable<Entity> LazilyFetch(EntityFetchRequest request)
        {
            using (IDbCommand command = CreateSelectCommand(request))
            {
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Entity entity = request.EntityType.CreateInstance();
                        entity.Load(reader);
                        yield return entity;
                    }
                }
            }
        }
    }
}
