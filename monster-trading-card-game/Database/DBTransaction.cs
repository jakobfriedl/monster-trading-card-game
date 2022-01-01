using System;
using System.Collections.Generic;
using System.Net.Sockets;
using monster_trading_card_game.Trade;
using Npgsql;

namespace monster_trading_card_game.Database {
    public class DBTransaction {
	    private readonly DBConnection dbConn = new DBConnection();

	    public bool NewTransaction(Transaction transaction) {
		    var conn = dbConn.Connect();

		    try {
			    using (var insertCmd =
				    new NpgsqlCommand(
					    "insert into \"transaction\"(user_id_1, user_id_2, card_id_1, card_id_2, coins, timestamp) values (@user1, @user2, @card1, @card2, @coins, @timestamp)",
					    conn)) {
				    insertCmd.Parameters.AddWithValue("user1", transaction.User1); 
				    insertCmd.Parameters.AddWithValue("user2", transaction.User2); 
				    insertCmd.Parameters.AddWithValue("card1", transaction.Card1); 
				    insertCmd.Parameters.AddWithValue("card2", transaction.Card2); 
				    insertCmd.Parameters.AddWithValue("coins", transaction.Coins); 
				    insertCmd.Parameters.AddWithValue("timestamp", transaction.Timestamp); 
					insertCmd.Prepare();

					if (insertCmd.ExecuteNonQuery() < 0) return false; 
			    }
		    } catch (PostgresException) {
			    return false; 
		    }

			conn.Close();
			return true; 
	    }

	    public List<Transaction> GetTransactionsByUserId(int id) {
		    var conn = dbConn.Connect();

		    var transactions = new List<Transaction>();

		    try {
			    using (var selectCmd = new NpgsqlCommand("select * from \"transaction\" where user_id_1=@id or user_id_2=@id order by timestamp asc", conn)) {
				    selectCmd.Parameters.AddWithValue("id", id); 
					selectCmd.Prepare();

					using (var reader = selectCmd.ExecuteReader()) {
						while (reader.Read()) {
							var t = new Transaction(
								(int)reader["transaction_id"],
								(int)reader["user_id_1"],
								(int)reader["user_id_2"],
								(int)reader["card_id_1"],
								(int)reader["card_id_2"],
								(int)reader["coins"],
								(int)reader["timestamp"]);

							transactions.Add(t);
						}
					}
				}
		    } catch (PostgresException) {
			    return null; 
		    }

			conn.Close(); 
			return transactions;
	    }
    }
}
