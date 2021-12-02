using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using monster_trading_card_game.Users;
using monster_trading_card_game.Database;
using Npgsql;

namespace monster_trading_card_game.Database {
    class DBUser {

	    private readonly DBConnection dbConn = new DBConnection();

		public bool RegisterUser(IUser user) {
		    var conn = dbConn.Connect();

			// Insert new User
			try {
				using (var userCmd = new NpgsqlCommand("insert into \"user\"(username, password, coins, elo, wins, losses) values (@username, @password, @coins, @elo, @wins, @losses)", conn)) {
					userCmd.Parameters.AddWithValue("username", user.Username);
					userCmd.Parameters.AddWithValue("password", user.Password);
					userCmd.Parameters.AddWithValue("coins", user.Coins);
					userCmd.Parameters.AddWithValue("elo", user.Elo);
					userCmd.Parameters.AddWithValue("wins", user.Wins);
					userCmd.Parameters.AddWithValue("losses", user.Losses);

					if (userCmd.ExecuteNonQuery() < 0) return false;
				}
			}
			catch (PostgresException) {
				return false; 
			}

			// Select latest User
			var getLatestUserCmd = new NpgsqlCommand("select user_id from \"user\" order by user_id desc limit 1", conn); 
		    int latestUser = (int)getLatestUserCmd.ExecuteScalar();

		    // Insert new Cards 
		    foreach (var card in user.CardStack.Cards) {
			    try {
				    using (var cardCmd = new NpgsqlCommand("insert into \"card\"(name, damage, element_type, monster_type, user_id, in_deck) values (@name, @damage, @element_type, @monster_type, @user_id, @in_deck)", conn)) {
					    cardCmd.Parameters.AddWithValue("name", card.Name);
					    cardCmd.Parameters.AddWithValue("damage", card.Damage);
					    cardCmd.Parameters.AddWithValue("element_type", (int)card.ElementType);
					    cardCmd.Parameters.AddWithValue("monster_type", (int)card.MonsterType);
					    cardCmd.Parameters.AddWithValue("user_id", latestUser);
					    // Check if Card is in Deck
					    if (user.Deck.Cards.Contains(card)) {
						    cardCmd.Parameters.AddWithValue("in_deck", true);
					    } else {
						    cardCmd.Parameters.AddWithValue("in_deck", false);
					    }

					    if (cardCmd.ExecuteNonQuery() < 0) return false;
				    }
			    }catch(PostgresException) {
				    return false; 
			    }
			}

		    conn.Close(); 
		    return true; 
		}

		//public IUser LoginUser(string username, string password) {
		//	var conn = dbConn.Connect(); 

		//	// Get User by Username
		//	try {
		//		var cmd = new NpgsqlCommand("select * from \"user\" where username=@username", conn);
		//		cmd.Parameters.AddWithValue("username", username);

		//		using (var reader = cmd.ExecuteReader()) {
		//			while (reader.Read()) {
		//				// Console.WriteLine($"{reader["username"]}\t{reader["password"]}");
		//				// Check if Passwords match
		//				if (password != (string)reader["password"]) return null;


		//			}
		//		}

		//	} catch (PostgresException) {
		//		return null;
		//	}


		//	conn.Close(); 
		//}
	}
}
