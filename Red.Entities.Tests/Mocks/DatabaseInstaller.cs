using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations.Model;

namespace Red.Entities.Tests
{
    public class DatabaseInstaller
    {
        public IDbConnection Connection { get; private set; }
        public Database Database { get; private set; }
        public DatabaseInstaller(IDbConnection connection, Database database)
        {
            Connection = connection;
            Database = database;
        }
		
        private IDbCommand CreateCommand(string commandText)
        {
            return CreateCommand(commandText, new Dictionary<string, object>());
        }

        private IDbCommand CreateCommand(string commandText, Dictionary<string, object> arguments)
        {
            var command = Connection.CreateCommand();
            command.CommandText = commandText;
            foreach (string key in arguments.Keys)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = key;
                parameter.Value = arguments[key] ?? DBNull.Value;
                command.Parameters.Add(parameter);
            }
            return command;
        }

        private string CreateTable(string name, string[] columns)
        {
            return $"CREATE TABLE {name}\n (" + string.Join(",\n\t", columns) + ");";
        }
        private string CreateColumn(string name, string spec)
        {
            return $"{name} {spec}";
        }

        public void Install()
        {
            CreateCommand(
                CreateTable("Customer", new []
                {
                    CreateColumn("_id", "INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT"),
                    CreateColumn("name", "VARCHAR(100)"),
                })
            ).ExecuteNonQuery();
            CreateCommand(
                CreateTable("Invoice", new []
                {
                    CreateColumn("_id", "INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT"),
                    CreateColumn("_Customer", "INTEGER NOT NULL"),
                })
            ).ExecuteNonQuery();
            CreateCommand(
                CreateTable("InvoiceLine", new []
                {
                    CreateColumn("_id", "INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT"),
                    CreateColumn("_Invoice", "INTEGER NOT NULL"),
                    CreateColumn("Text", "VARCHAR(200) NOT NULL"),
                    CreateColumn("Code", "VARCHAR(16)"),
                    CreateColumn("Units", "DECIMAL(10,2)"),
                    CreateColumn("UnitPrice", "DECIMAL(10,2)"),
                    CreateColumn("_TaxRate", "INTEGER NOT NULL"),
                    CreateColumn("TaxPercentage", "DECIMAL(10,2)"),
                    CreateColumn("TaxAmount", "DECIMAL(10,2)"),
                    CreateColumn("AmountExcTax", "DECIMAL(10,2)"),
                    CreateColumn("AmountIncTax", "DECIMAL(10,2)"),
                })
            ).ExecuteNonQuery();
        }
        
        
    }
}