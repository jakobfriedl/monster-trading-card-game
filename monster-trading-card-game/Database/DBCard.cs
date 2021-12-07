using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using monster_trading_card_game.CardCollections;
using monster_trading_card_game.Cards;
using monster_trading_card_game.Enums;
using Npgsql; 


namespace monster_trading_card_game.Database {
    class DBCard {
	    private readonly DBConnection dbConn = new DBConnection();

	    public CardStack GetCardStackFromId(int id) {
		    var conn = dbConn.Connect(); 

		    CardStack stack = new CardStack(); 

		    try {
				var cardCmd = new NpgsqlCommand("select * from \"card\" where user_id=@user_id", conn);
			    cardCmd.Parameters.AddWithValue("user_id", id);
			    cardCmd.Prepare();

			    using (var reader = cardCmd.ExecuteReader()) {
				    while (reader.Read()) {
					    if ((int)reader["monster_type"] == 0) {
						    // Spell
						    Spell spell = new Spell(
							    (string)reader["name"],
							    (int)reader["damage"],
							    (ElementType)reader["element_type"]);

						    if (!(bool)reader["in_deck"]) {
							    stack.AddCard(spell);
						    }
					    } else {
						    // Monster
						    Monster monster = new Monster(
							    (string)reader["name"],
							    (int)reader["damage"],
							    (ElementType)reader["element_type"],
							    (MonsterType)reader["monster_type"]);

						    if (!(bool)reader["in_deck"]) {
							    stack.AddCard(monster);
						    }
					    }
				    }
			    }
		    } catch (PostgresException) {
			    return null;
		    }

		    return stack; 
	    }

	    public Deck GetDeckFromId(int id) {
		    var conn = dbConn.Connect(); 
		    Deck deck = new Deck();

		    try {
			    var cardCmd = new NpgsqlCommand("select * from \"card\" where user_id=@user_id", conn);
			    cardCmd.Parameters.AddWithValue("user_id", id);
			    cardCmd.Prepare();

			    using (var reader = cardCmd.ExecuteReader()) {
				    while (reader.Read()) {
					    if ((int)reader["monster_type"] == 0) {
						    // Spell
						    Spell spell = new Spell(
							    (string)reader["name"],
							    (int)reader["damage"],
							    (ElementType)reader["element_type"]);

						    if ((bool)reader["in_deck"]) {
							    deck.AddCard(spell);
						    }
					    } else {
						    // Monster
						    Monster monster = new Monster(
							    (string)reader["name"],
							    (int)reader["damage"],
							    (ElementType)reader["element_type"],
							    (MonsterType)reader["monster_type"]);

						    if ((bool)reader["in_deck"]) {
							    deck.AddCard(monster);
						    }
					    }
				    }
			    }
		    } catch (PostgresException) {
			    return null;
		    }

		    return deck; 
	    }
    }
}
