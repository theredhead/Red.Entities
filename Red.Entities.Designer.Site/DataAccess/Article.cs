using System;
using System.Data;
using Red.Entities;
using MySql.Data.MySqlClient;

namespace Red.Entities.Designer.Site
{

    class Article : Entity
    {
        public string Code
        {
            get { return (string)this["Code"]; }
            set { this["Code"] = value; }
        }

        public decimal UnitPrice
        {
            get { return (decimal)this["UnitPrice"]; }
            set { this["UnitPrice"] = value; }
        }

        public string Description
        {
            get { return (string)this["Description"]; }
            set { this["Description"] = value; }
        }

        public Article(Articles table) : base(table)
        {
        }
    }

}
