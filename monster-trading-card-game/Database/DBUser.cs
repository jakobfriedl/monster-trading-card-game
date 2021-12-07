using Castle.DynamicProxy.Generators;
using monster_trading_card_game.CardCollections;
using monster_trading_card_game.Cards;
using monster_trading_card_game.Users;
using monster_trading_card_game.Enums;
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
					userCmd.Prepare();
					if (userCmd.ExecuteNonQuery() < 0) return false;
				}
			} catch (PostgresException) {
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

					    cardCmd.Prepare();
					    if (cardCmd.ExecuteNonQuery() < 0) return false;
				    }
			    } catch(PostgresException) {
				    return false; 
			    }
			}

		    conn.Close(); 
		    return true; 
		}

        public IUser LoginUser(string username, string password) {
            var conn = dbConn.Connect();

            User user = new User();

            // Get User by Username
            try {
                var userCmd = new NpgsqlCommand("select * from \"user\" where username=@username", conn);
                userCmd.Parameters.AddWithValue("username", username);
                userCmd.Prepare();
                using (var reader = userCmd.ExecuteReader()) {
                    while (reader.Read()) {
	                    user = new User(
							(int)reader["user_id"],
		                    (string)reader["username"], 
		                    (string)reader["password"], 
		                    (int)reader["coins"],
		                    (int)reader["elo"],
		                    (int)reader["wins"], 
		                    (int)reader["losses"]); 
                    }
				}
            } catch (PostgresException) {
                return null;
            }

            var dbCard = new DBCard();

            user.CardStack = dbCard.GetCardStackFromId(user.Id);
            user.Deck = dbCard.GetDeckFromId(user.Id);

            conn.Close();

            return user;
        }

        public bool UpdateStats(IUser user) {
	        var conn = dbConn.Connect();

	        try {
		        var updateUserCmd = new NpgsqlCommand("update \"user\" set wins=@wins, losses=@losses, elo=@elo where user_id=@user_id", conn);
		        updateUserCmd.Parameters.AddWithValue("wins", user.Wins);
		        updateUserCmd.Parameters.AddWithValue("losses", user.Losses);
		        updateUserCmd.Parameters.AddWithValue("elo", user.Elo);
		        updateUserCmd.Parameters.AddWithValue("user_id", user.Id);
				updateUserCmd.Prepare();

				updateUserCmd.ExecuteNonQuery(); 

	        } catch (PostgresException) {
		        return false; 
	        }

	        return true; 
        }
    }
}
