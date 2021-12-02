using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using monster_trading_card_game.Users;
using Npgsql;

namespace monster_trading_card_game.Database {
	class DBConnection {

		const string ConnString = "Host=localhost;Username=postgres;Password=postgres;Database=mtcgdb";
		private NpgsqlConnection connection; 

		public NpgsqlConnection Connect() {
			connection = new NpgsqlConnection(ConnString);
			connection.Open();
			return connection;
		}
	}
}

