using System;
using System.Data;
using Red.Entities;
using MySql.Data.MySqlClient;

namespace Red.Entities.Designer.Site
{
    public class Articles : EntityType
    {
        public Articles(EntityDatabase database, string tableName)
            : base(database, tableName, new string[] { "_Id", "Code", "UnitPrice", "Description" })
        {
            InstanceType = typeof(Article);
        }
    }

}
