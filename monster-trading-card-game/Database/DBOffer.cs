using System;
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
			    using (var offerCmd = new NpgsqlCommand("insert into \"offer\"(user_id, card_id, element, monster, min_damage, price) values (@user_id, @card_id, @element, @monster, @min_damage, @price)", conn)) {
				    offerCmd.Parameters.AddWithValue("user_id", offer.UserId);
				    offerCmd.Parameters.AddWithValue("card_id", offer.CardId);
				    offerCmd.Parameters.AddWithValue("element", offer.Element);
				    offerCmd.Parameters.AddWithValue("monster", offer.Monster);
				    offerCmd.Parameters.AddWithValue("min_damage", offer.MinDamage);
				    offerCmd.Parameters.AddWithValue("price", offer.Price);
				    offerCmd.Prepare();

				    if (offerCmd.ExecuteNonQuery() < 0) return false;
			    }
		    } catch (PostgresException e) {
			    Console.WriteLine(e);
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
								(int)reader["element"],
								(int)reader["monster"],
								(int)reader["min_damage"],
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
								(int)reader["element"],
								(int)reader["monster"],
								(int)reader["min_damage"],
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

	    public bool RemoveOfferByOfferId(int id) {
		    var conn = dbConn.Connect();

		    try {
			    using (var deleteOfferCmd = new NpgsqlCommand("delete from \"offer\" where offer_id=@offer_id", conn)) {
				    deleteOfferCmd.Parameters.AddWithValue("offer_id", id);
					deleteOfferCmd.Prepare();

					if (deleteOfferCmd.ExecuteNonQuery() < 0) return false;
			    }
		    } catch (PostgresException) {
			    return false; 
		    }

			conn.Close();
		    return true;
	    }

	    public bool UpdateOffer(int offerId, Offer newOffer) {
		    var conn = dbConn.Connect();

		    try {
			    using (var updateCmd = new NpgsqlCommand("update \"offer\" set element=@element, monster=@monster, min_damage=@min_damage, price=@price where offer_id=@offer_id", conn)) {
				    updateCmd.Parameters.AddWithValue("element", newOffer.Element);
				    updateCmd.Parameters.AddWithValue("monster", newOffer.Monster);
				    updateCmd.Parameters.AddWithValue("min_damage", newOffer.MinDamage);
				    updateCmd.Parameters.AddWithValue("price", newOffer.Price);
				    updateCmd.Parameters.AddWithValue("offer_id", offerId);
					updateCmd.Prepare();

					if (updateCmd.ExecuteNonQuery() < 0) return false;
			    }
			} catch (PostgresException) {
			    return false; 
		    }

		    conn.Close();
		    return true;
	    }
    }
}
