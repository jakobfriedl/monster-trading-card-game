using System.Collections.Generic;
using System.Linq.Expressions;
using monster_trading_card_game.Trade;
using Npgsql;

namespace monster_trading_card_game.Database {
    public class DBOffer {
	    private readonly DBConnection dbConn = new DBConnection();

	    public bool CreateNewOffer(Offer offer) {
		    var conn = dbConn.Connect();

		    try {
			    using (var offerCmd = new NpgsqlCommand("insert into \"offer\"(user_id, card_id, price) values (@user_id, @card_id, @price)", conn)) {
				    offerCmd.Parameters.AddWithValue("user_id", offer.UserId);
				    offerCmd.Parameters.AddWithValue("card_id", offer.CardId);
				    offerCmd.Parameters.AddWithValue("price", offer.Price);
				    offerCmd.Prepare();

				    if (offerCmd.ExecuteNonQuery() < 0) return false;
			    }
		    } catch (PostgresException) {
			    return false;
		    }

			conn.Close();
		    return true;
	    }

	    public List<Offer> GetOffersByUserId(int id) {
		    var conn = dbConn.Connect();

		    var offers = new List<Offer>();

		    try {
			    using (var offerCmd = new NpgsqlCommand("select * from \"offer\" where user_id=@user_id", conn)) {
				    offerCmd.Parameters.AddWithValue("user_id", id);
				    offerCmd.Prepare();

				    using (var reader = offerCmd.ExecuteReader()) {
					    while (reader.Read()) {
						    var offer = new Offer(
							    (int)reader["offer_id"],
							    (int)reader["user_id"],
							    (int)reader["card_id"],
							    (int)reader["price"]);

						    offers.Add(offer);
					    }
				    }
			    }
		    } catch (PostgresException) {
			    return null; 
		    }

		    conn.Close();
		    return offers; 
	    }

	    public List<Offer> GetOffersFromOtherUsers(int id) {
		    var conn = dbConn.Connect();

		    var offers = new List<Offer>();

		    try {
			    using (var offerCmd = new NpgsqlCommand("select * from \"offer\" where user_id!=@user_id", conn)) {
				    offerCmd.Parameters.AddWithValue("user_id", id);
				    offerCmd.Prepare();

				    using (var reader = offerCmd.ExecuteReader()) {
					    while (reader.Read()) {
						    var offer = new Offer(
							    (int)reader["offer_id"],
							    (int)reader["user_id"],
							    (int)reader["card_id"],
							    (int)reader["price"]);

						    offers.Add(offer);
					    }
				    }
			    }
		    } catch (PostgresException) {
			    return null;
		    }

			conn.Close();
		    return offers;
		}
    }
}
