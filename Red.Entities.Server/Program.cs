using System;
using System.Net;
using System.Net.Sockets;
using Red.Entities;
using System.Data;
using MySql.Data.MySqlClient;
using Red.Entities.Web;

namespace Red.Entities.Server
{
	class Program
	{
		const string connectionString = "server=localhost;database=Entities;uid=entities;password=entities;";

		public static void Main()
		{
			IDbConnection connection = new MySqlConnection(connectionString);
			EntityStatementBuilder builder = new MySqlEntityStatementBuilder();
			EntityDatabase database = new EntityDatabase(connection, builder);
			EntityServer<EntityDatabase> server = new EntityServer<EntityDatabase>(database);

			server.Run(IPAddress.Any, 8181);
		}
	}
}
