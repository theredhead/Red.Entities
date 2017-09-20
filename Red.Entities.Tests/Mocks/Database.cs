using System.Data;
using System.Data.SQLite;

namespace Red.Entities.Tests.Mocka
{
	public class Database : EntityDatabase
	{
		public Database() : this(
			new SQLiteConnection("Data Source=:memory:;Version=3;New=True;"),
			new MySqlEntityStatementBuilder())
		{
		}

		protected Database(IDbConnection connection, EntityStatementBuilder statementBuilder) : base(connection, statementBuilder)
		{
			(new DatabaseInstaller(connection)).Install();
		}
	}
}