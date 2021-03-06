using System.Collections.Generic;
using monster_trading_card_game.CardCollections;
using monster_trading_card_game.Users;
using monster_trading_card_game.Security;
using Npgsql;

namespace monster_trading_card_game.Database {
    class DBUser {

	    private readonly PasswordHasher hasher = new PasswordHasher();
	    private readonly DBConnection dbConn = new DBConnection();

		/// <summary>
		/// Registers and users and inserts Starter-Cards to Database
		/// </summary>
		/// <param name="user"> new User Object </param>
		/// <returns> true if registration successful </returns>
		/// <returns> false if registration fails </returns>
		public bool RegisterUser(IUser user) {
		    var conn = dbConn.Connect();

		    var hashedPassword = hasher.Hash(user.Password);

		    // Insert new User
			try {
				using (var userCmd = new NpgsqlCommand("insert into \"user\"(username, password, coins, elo, wins, losses) values (@username, @password, @coins, @elo, @wins, @losses)", conn)) {
					userCmd.Parameters.AddWithValue("username", user.Username);
					userCmd.Parameters.AddWithValue("password", hashedPassword);
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

		/// <summary>
		/// Checks for username in database and verifies password
		/// </summary>
		/// <param name="username"> login-username </param>
		/// <param name="password"> login-password </param>
		/// <returns> User object with Cards and Deck if login successful </returns>
		/// <returns> null if login fails </returns>
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
	                    if (!hasher.Verify(password, (string)reader["password"])) return null; 
	                    
	                    user = new User(
							(int)reader["user_id"],
		                    (string)reader["username"], 
		                    password, 
		                    (int)reader["coins"],
		                    (int)reader["elo"],
		                    (int)reader["wins"], 
		                    (int)reader["losses"]);

                    }
				}
            } catch (PostgresException) {
                return null;
            }
            if (user.Username == null) return null;

			var dbCard = new DBCard();

            user.CardStack = dbCard.GetCardStackFromUserId(user.Id);
            user.Deck = dbCard.GetDeckFromUserId(user.Id);

            conn.Close();
            return user;
        }

        public string GetUsernameByUserId(int id) {
	        var conn = dbConn.Connect();

	        string username; 
	        // Get Username by ID
	        try {
		        var userCmd = new NpgsqlCommand("select username from \"user\" where user_id=@user_id", conn);
		        userCmd.Parameters.AddWithValue("user_id", id);
		        userCmd.Prepare();

		        username = (string)userCmd.ExecuteScalar(); 
	        } catch (PostgresException) {
		        return null;
	        }

	        conn.Close();
	        return username;
		}

        public IUser GetUserObjectByUserId(int id) {
	        var conn = dbConn.Connect();

	        IUser user = new User();
	        // Get Username by ID
	        try {
		        var userCmd = new NpgsqlCommand("select * from \"user\" where user_id=@user_id", conn);
		        userCmd.Parameters.AddWithValue("user_id", id);
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

	        user.CardStack = dbCard.GetCardStackFromUserId(user.Id);
	        user.Deck = dbCard.GetDeckFromUserId(user.Id);

			conn.Close();
	        return user;
        }

		/// <summary>
		/// Update Stats after a battle
		/// </summary>
		/// <param name="user"> user object </param>
		/// <returns> true if update successful </returns>
		/// <returns> false if update fails </returns>
		public bool UpdateStats(IUser user) {
	        var conn = dbConn.Connect();

	        try {
		        var updateUserCmd = new NpgsqlCommand("update \"user\" set wins=@wins, losses=@losses, elo=@elo, coins=@coins where user_id=@user_id", conn);
		        updateUserCmd.Parameters.AddWithValue("wins", user.Wins);
		        updateUserCmd.Parameters.AddWithValue("losses", user.Losses);
		        updateUserCmd.Parameters.AddWithValue("elo", user.Elo);
		        updateUserCmd.Parameters.AddWithValue("coins", user.Coins);
		        updateUserCmd.Parameters.AddWithValue("user_id", user.Id);
		        updateUserCmd.Prepare();

				if (updateUserCmd.ExecuteNonQuery() < 0) return false;

	        } catch (PostgresException) {
		        return false; 
	        }

	        return true; 
        }

		/// <summary>
		/// Change Password of specific user
		/// </summary>
		/// <param name="id"> Id of User </param>
		/// <param name="oldPassword"> old password, must be verified with database </param>
		/// <param name="newPassword"> new password </param>
		/// <param name="repeatPassword"> repeated new password, must match newPassword</param>
		/// <returns> true if change successful </returns>
		/// <returns> false if change fails </returns>
        public bool ChangePassword(int id, string oldPassword, string newPassword, string repeatPassword) {
	        var conn = dbConn.Connect();

	        // Verify old password
			try {
				using (var selectOldPassword = new NpgsqlCommand("select password from \"user\" where user_id=@user_id", conn)) {
					selectOldPassword.Parameters.AddWithValue("user_id", id); 
					selectOldPassword.Prepare();

					string dbPass = (string)selectOldPassword.ExecuteScalar();

					if (!hasher.Verify(oldPassword, dbPass)) return false; // Return if old password has not been entered correctly
				}
			} catch (PostgresException) {
				return false;
			}

			string newHashed = hasher.Hash(newPassword);
			if (!hasher.Verify(repeatPassword, newHashed)) return false; // Return if new password has not been repeated correctly

			// Update Password
			try {
				using (var updateCmd = new NpgsqlCommand("update \"user\" set password=@new where user_id=@user_id", conn)) {
					updateCmd.Parameters.AddWithValue("new", newHashed);
					updateCmd.Parameters.AddWithValue("user_id", id);
					updateCmd.Prepare();

					if (updateCmd.ExecuteNonQuery() < 0) return false;
				}
			} catch (PostgresException) {
		        return false; 
	        }

	        conn.Close();
	        return true;
        }

		/// <summary>
		/// Buy Package, Update coins and add package-cards to Stack
		/// </summary>
		/// <param name="package"> Package object </param>
		/// <param name="user"> User object </param>
		/// <returns> true if successful </returns>
		/// <returns> false if fail </returns>
        public bool BuyPackage(Package package, IUser user) {
	        var conn = dbConn.Connect();

	        try {
		        var updateUserCmd = new NpgsqlCommand("update \"user\" set coins=@coins where user_id=@user_id", conn);
		        updateUserCmd.Parameters.AddWithValue("coins", user.Coins);
		        updateUserCmd.Parameters.AddWithValue("user_id", user.Id);
		        updateUserCmd.Prepare();
		        
		        if(updateUserCmd.ExecuteNonQuery() < 0) return false;

	        } catch (PostgresException) {
		        return false;
	        }

	        var dbCard = new DBCard();

			conn.Close();
	        return dbCard.AddPackageToCards(package, user.Id);
		}

		/// <summary>
		/// Get a list of all users
		/// </summary>
		/// <returns> List of users ordered by elo </returns>
        public List<IUser> GetAllUsers() {
	        var conn = dbConn.Connect();

	        var users = new List<IUser>();

	        try {
		        var selectUsersCmd = new NpgsqlCommand("select * from \"user\" order by elo desc", conn);

		        using (var reader = selectUsersCmd.ExecuteReader()) {
			        while (reader.Read()) {

				        var stats = new User(
					        (int)reader["user_id"],
					        (string)reader["username"],
					        (string)reader["password"],
					        (int)reader["coins"],
					        (int)reader["elo"],
					        (int)reader["wins"],
					        (int)reader["losses"]);

				        users.Add(stats);
			        }
		        }
	        }
	        catch (PostgresException) {
		        return null; 
	        }

	        conn.Close(); 
	        return users; 
        }

		/// <summary>
		/// Get Number of Coins by User
		/// </summary>
		/// <param name="id"> User-ID </param>
		/// <returns> Number of coins, -1 on error </returns>
        public int GetCoinsByUserId(int id) {
	        var conn = dbConn.Connect();
	        int coins = -1;

	        try {
		        using (var cmd = new NpgsqlCommand("select coins from \"user\" where user_id=@user_id", conn)) {
			        cmd.Parameters.AddWithValue("user_id", id);
					cmd.Prepare();

					coins = (int)cmd.ExecuteScalar(); 
		        }
	        } catch (PostgresException) {
		        return -1; 
	        }

	        conn.Close();
	        return coins;
        }

		/// <summary>
		/// Move coins from one user to another
		/// </summary>
		/// <param name="sender"> ID of user that looses coins </param>
		/// <param name="receiver"> ID of user that receives coins </param>
		/// <param name="coins"> Number of coins </param>
		/// <returns> true if transfer is successful </returns>
		/// <returns> false if transfer is fails </returns>
        public bool TransferCoins(IUser sender, int receiver, int coins) {
	        var conn = dbConn.Connect();

			// Update Sender
	        try {
		        using (var senderCmd = new NpgsqlCommand("update \"user\" set coins=@coins where user_id=@user_id", conn)) {
			        senderCmd.Parameters.AddWithValue("coins", sender.Coins); // Already updated before 
			        senderCmd.Parameters.AddWithValue("user_id", sender.Id); 
					senderCmd.Prepare();

					if (senderCmd.ExecuteNonQuery() < 0) return false;
		        }
			} catch (PostgresException) {
		        return false; 
	        }

	        // Update Receiver
	        try {
		        using (var receiverCommand = new NpgsqlCommand("update \"user\" set coins=@coins where user_id=@user_id", conn)) {
			        receiverCommand.Parameters.AddWithValue("coins", GetCoinsByUserId(receiver)+coins);
			        receiverCommand.Parameters.AddWithValue("user_id", receiver);
			        receiverCommand.Prepare();

			        if (receiverCommand.ExecuteNonQuery() < 0) return false;
		        }
	        } catch (PostgresException) {
		        return false;
	        }


			conn.Close(); 
	        return true;
        }
    }
}
