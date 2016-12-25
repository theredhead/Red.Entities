using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MySql.Data.MySqlClient;
using System.Data;
using System;
using System.Web.Configuration;
using System.Web.Http;

namespace Red.Entities.Designer.Site
{
	public class Global : HttpApplication
	{
		static private Global _instance;
		static internal Global Current
		{
			get
			{
				return _instance;
			}
		}

		private EntitiesDatabase _database;
		internal EntitiesDatabase Database
		{
			get
			{
				if (_database == null)
				{
					_database = CreateDatabase();
				}
				return _database;
			}
		}

		private EntitiesDatabase CreateDatabase()
		{
			var connectionString = WebConfigurationManager.ConnectionStrings["default"];
			IDbConnection connection;
			EntityStatementBuilder builder;


			switch (connectionString.ProviderName)
			{
				case "MySql.Data.MySqlClient":
					connection = new MySqlConnection(connectionString.ConnectionString);
					builder = new MySqlEntityStatementBuilder();
					break;
				default:
					throw new NotSupportedException(connectionString.ProviderName);
			}

			return new EntitiesDatabase(connection, builder);
		}

		protected void Application_Start()
		{
			_instance = this;
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
		}
	}
}
