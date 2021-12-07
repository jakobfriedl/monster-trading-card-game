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
				var cardCmd = new NpgsqlCommand("select * from \"card\" where user_id=@user_id and in_deck=@in_deck", conn);
			    cardCmd.Parameters.AddWithValue("user_id", id);
			    cardCmd.Parameters.AddWithValue("in_deck", false);
			    cardCmd.Prepare();

			    using (var reader = cardCmd.ExecuteReader()) {
				    while (reader.Read()) {
					    if ((int)reader["monster_type"] == 0) {
						    // Spell
						    Spell spell = new Spell(
							    (int)reader["card_id"],
							    (string)reader["name"],
							    (int)reader["damage"],
							    (ElementType)reader["element_type"]);

						    stack.AddCard(spell);
					    } else {
						    // Monster
						    Monster monster = new Monster(
								(int)reader["card_id"],
							    (string)reader["name"],
							    (int)reader["damage"],
							    (ElementType)reader["element_type"],
							    (MonsterType)reader["monster_type"]);

						    stack.AddCard(monster);
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
			    var cardCmd = new NpgsqlCommand("select * from \"card\" where user_id=@user_id and in_deck=@in_deck", conn);
			    cardCmd.Parameters.AddWithValue("user_id", id);
			    cardCmd.Parameters.AddWithValue("in_deck", true);
			    cardCmd.Prepare();

			    using (var reader = cardCmd.ExecuteReader()) {
				    while (reader.Read()) {
					    if ((int)reader["monster_type"] == 0) {
						    // Spell
						    Spell spell = new Spell(
							    (int)reader["card_id"],
							    (string)reader["name"],
							    (int)reader["damage"],
							    (ElementType)reader["element_type"]);

						    deck.AddCard(spell);
					    } else {
						    // Monster
						    Monster monster = new Monster(
							    (int)reader["card_id"],
							    (string)reader["name"],
							    (int)reader["damage"],
							    (ElementType)reader["element_type"],
							    (MonsterType)reader["monster_type"]);

						    deck.AddCard(monster);
					    }
				    }
			    }
		    } catch (PostgresException) {
			    return null;
		    }

		    return deck; 
	    }

	    public CardStack GetAllCardsFromId(int id) {
		    var conn = dbConn.Connect();
		    CardStack cards = new CardStack();

		    try {
			    var cardCmd = new NpgsqlCommand("select * from \"card\" where user_id=@user_id", conn);
			    cardCmd.Parameters.AddWithValue("user_id", id);
			    cardCmd.Prepare();

			    using (var reader = cardCmd.ExecuteReader()) {
				    while (reader.Read()) {
					    if ((int)reader["monster_type"] == 0) {
						    // Spell
						    Spell spell = new Spell(
							    (int)reader["card_id"],
							    (string)reader["name"],
							    (int)reader["damage"],
							    (ElementType)reader["element_type"]);

							    cards.AddCard(spell);
						    } else {
						    // Monster
						    Monster monster = new Monster(
							    (int)reader["card_id"],
							    (string)reader["name"],
							    (int)reader["damage"],
							    (ElementType)reader["element_type"],
							    (MonsterType)reader["monster_type"]);

							    cards.AddCard(monster);
					    }
				    }
			    }
		    } catch (PostgresException) {
			    return null;
		    }

		    return cards;
	    }

	    public int GetCardOwner(int cardID) {
		    var conn = dbConn.Connect();

		    int owner = -1;
		    try {
			    var cardCmd = new NpgsqlCommand("select user_id from \"card\" where card_id=@card_id", conn);
			    cardCmd.Parameters.AddWithValue("card_id", cardID);
			    cardCmd.Prepare();

			    using (var reader = cardCmd.ExecuteReader()) {
				    while (reader.Read()) {
					    owner = (int)reader["user_id"]; 
				    }
			    } 

		    } catch (NullReferenceException) {
			    return -1; 
		    }

		    return owner; 
	    }

	    public ICard GetCardByCardId(int cardID) {
		    var conn = dbConn.Connect();

		    try {
			    var cardCmd = new NpgsqlCommand("select * from \"card\" where card_id=@card_id", conn);
			    cardCmd.Parameters.AddWithValue("card_id", cardID);
			    cardCmd.Prepare();

			    using (var reader = cardCmd.ExecuteReader()) {
				    while (reader.Read()) {
					    if ((int)reader["monster_type"] == 0) {
						    // Spell
						    Spell spell = new Spell(
							    (int)reader["card_id"],
							    (string)reader["name"],
							    (int)reader["damage"],
							    (ElementType)reader["element_type"]);

						    return spell;
					    }

					    // Monster
					    Monster monster = new Monster(
						    (int)reader["card_id"],
						    (string)reader["name"],
						    (int)reader["damage"],
						    (ElementType)reader["element_type"],
						    (MonsterType)reader["monster_type"]);

					    return monster;
				    }
			    }
		    } catch (PostgresException) {
			    return null;
		    }

		    return null; 
	    }

	    public bool UpdateDeck(int userId, Deck deck) {
		    var conn = dbConn.Connect();

		    try {
			    var resetDeckCmd = new NpgsqlCommand("update \"card\" set in_deck=@new where in_deck=@old and user_id=@user_id", conn);
			    resetDeckCmd.Parameters.AddWithValue("new", false);
			    resetDeckCmd.Parameters.AddWithValue("old", true);
			    resetDeckCmd.Parameters.AddWithValue("user_id", userId);
			    resetDeckCmd.Prepare();

			    resetDeckCmd.ExecuteNonQuery();
		    } catch (PostgresException) {
			    return false; 
		    }

		    foreach (var card in deck.Cards) {
			    try {
				    var updateDeckCmd = new NpgsqlCommand("update \"card\" set in_deck=@new where in_deck=@old and card_id=@card_id", conn);
				    updateDeckCmd.Parameters.AddWithValue("new", true);
				    updateDeckCmd.Parameters.AddWithValue("old", false);
				    updateDeckCmd.Parameters.AddWithValue("card_id", card.Id);
				    updateDeckCmd.Prepare();

				    updateDeckCmd.ExecuteNonQuery();
			    } catch {
				    return false;
			    }
		    }

		    return true;
	    }
	}
}
