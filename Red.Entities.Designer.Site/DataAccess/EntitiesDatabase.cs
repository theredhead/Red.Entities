using System;
using System.Data;
using Red.Entities;
using MySql.Data.MySqlClient;

namespace Red.Entities.Designer.Site
{
    public class EntitiesDatabase : EntityDatabase
    {
        public Articles Articles
        {
            get
            {
                return (Articles)Tables["Articles"];
            }
        }

        public EntitiesDatabase(IDbConnection connection, EntityStatementBuilder statementBuilder)
            : base(connection, statementBuilder)
        {
            Connection.Open();

            Tables["Articles"] = new Articles(this, "Article");
        }
    }
}
