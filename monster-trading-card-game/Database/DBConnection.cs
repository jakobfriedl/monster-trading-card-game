using Npgsql;

namespace monster_trading_card_game.Database {
	class DBConnection {

		const string ConnString = "Host=localhost;Username=postgres;Password=postgres;Database=mtcgdb";
		private NpgsqlConnection _connection; 

		public NpgsqlConnection Connect() {
			_connection = new NpgsqlConnection(ConnString);
			_connection.Open();
			return _connection;
		}
	}
}

