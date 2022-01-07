using System.Collections.Generic;
using monster_trading_card_game.Battle;
using Npgsql;

namespace monster_trading_card_game.Database {
    class DBBattle {
	    private readonly DBConnection dbConn = new DBConnection();

	    public bool CreateBattleRequest(BattleRequest req) {
		    var conn = dbConn.Connect();

		    try {
			    using (var insertCmd =
				    new NpgsqlCommand("insert into \"battle\"(user_id_1, user_id_2, completed) values (@user1, @user2, @completed)", conn)) {
				    insertCmd.Parameters.AddWithValue("@user1", req.User1);
				    insertCmd.Parameters.AddWithValue("@user2", req.User2); 
				    insertCmd.Parameters.AddWithValue("@completed", req.Completed);
					insertCmd.Prepare();

					if (insertCmd.ExecuteNonQuery() < 0) return false; 
			    }
		    } catch (PostgresException) {
			    return false; 
		    }

		    conn.Close();
			return true; 
	    }

	    public List<BattleRequest> GetAllOpenRequestsByUserId(int id) {
		    var conn = dbConn.Connect();

		    var requests = new List<BattleRequest>();

		    try {
			    using (var selectCmd = new NpgsqlCommand("select * from \"battle\" where (user_id_1=@id or user_id_2=@id) and completed=@false", conn)) {
					selectCmd.Parameters.AddWithValue("id", id);
					selectCmd.Parameters.AddWithValue("false", false);
					selectCmd.Prepare();

					using (var reader = selectCmd.ExecuteReader()) {
						while (reader.Read()) {
							var req = new BattleRequest(
								(int)reader["battle_id"],
								(int)reader["user_id_1"],
								(int)reader["user_id_2"],
								(bool)reader["completed"]);

							requests.Add(req);
						}
					}
				}
		    } catch (PostgresException) {
			    return null; 
		    }

			conn.Close();
			return requests; 
	    }

	    public bool RemoveBattleRequestById(int id) {
		    var conn = dbConn.Connect();

		    try {
			    using (var deleteCmd = new NpgsqlCommand("delete from \"battle\" where battle_id=@id", conn)) {
				    deleteCmd.Parameters.AddWithValue("id", id); 
					deleteCmd.Prepare();

					if (deleteCmd.ExecuteNonQuery() < 0) return false; 
			    }
		    } catch (PostgresException) {
			    return false; 
		    }

		    conn.Close();
		    return true;
	    }
    }
}
