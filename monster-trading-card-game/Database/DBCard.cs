using System;
using monster_trading_card_game.CardCollections;
using monster_trading_card_game.Cards;
using monster_trading_card_game.Enums;
using monster_trading_card_game.Users;
using Npgsql; 


namespace monster_trading_card_game.Database {
    class DBCard {
	    private readonly DBConnection dbConn = new DBConnection();

	    public CardStack GetCardStackFromUserId(int id) {
		    var conn = dbConn.Connect(); 

		    CardStack stack = new CardStack(); 

		    try {
				// Only select cards that are not used in the deck or in offers
				var cardCmd = new NpgsqlCommand("select * from \"card\" where user_id=@user_id and in_deck=@in_deck and card_id not in (select offer.card_id from( select card_id from offer where user_id=@user_id_2) as offer) order by damage desc", conn);
			    cardCmd.Parameters.AddWithValue("user_id", id);
			    cardCmd.Parameters.AddWithValue("in_deck", false);
			    cardCmd.Parameters.AddWithValue("user_id_2", id);
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

			conn.Close();
		    return stack; 
	    }

	    public Deck GetDeckFromUserId(int id) {
		    var conn = dbConn.Connect(); 
		    Deck deck = new Deck();

		    try {
			    var cardCmd = new NpgsqlCommand("select * from \"card\" where user_id=@user_id and in_deck=@in_deck order by damage desc", conn);
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

			conn.Close();
		    return deck; 
	    }

	    public CardStack GetAllCardsFromUserId(int id) {
		    var conn = dbConn.Connect();
		    CardStack cards = new CardStack();

		    try {
			    var cardCmd = new NpgsqlCommand("select * from \"card\" where user_id=@user_id and card_id not in (select offer.card_id from( select card_id from offer where user_id=@user_id_2) as offer) order by damage desc", conn);
			    cardCmd.Parameters.AddWithValue("user_id", id);
			    cardCmd.Parameters.AddWithValue("user_id_2", id);
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

		    conn.Close();
		    return cards;
	    }

	    public int GetCardOwner(int cardId) {
		    var conn = dbConn.Connect();

		    int owner = -1;
		    try {
			    var cardCmd = new NpgsqlCommand("select user_id from \"card\" where card_id=@card_id", conn);
			    cardCmd.Parameters.AddWithValue("card_id", cardId);
			    cardCmd.Prepare();

			    using (var reader = cardCmd.ExecuteReader()) {
				    while (reader.Read()) {
					    owner = (int)reader["user_id"]; 
				    }
			    } 

		    } catch (NullReferenceException) {
			    return -1; 
		    }

		    conn.Close();
		    return owner; 
	    }

	    public ICard GetCardByCardId(int cardId) {
		    var conn = dbConn.Connect();

		    try {
			    var cardCmd = new NpgsqlCommand("select * from \"card\" where card_id=@card_id", conn);
			    cardCmd.Parameters.AddWithValue("card_id", cardId);
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

			conn.Close();
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

				if (resetDeckCmd.ExecuteNonQuery() < 0) return false;
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

				    if (updateDeckCmd.ExecuteNonQuery() < 0) return false;
			    } catch {
				    return false;
			    }
		    }

		    conn.Close();
		    return true;
	    }

	    public bool AddPackageToCards(Package package, int userId) {
		    var conn = dbConn.Connect();

		    foreach (var card in package.Cards) {
			    try {
				    using (var addPackageCmd = new NpgsqlCommand("insert into \"card\"(name, damage, element_type, monster_type, user_id, in_deck) values (@name, @damage, @element_type, @monster_type, @user_id, @in_deck)", conn)) {
					    addPackageCmd.Parameters.AddWithValue("name", card.Name);
					    addPackageCmd.Parameters.AddWithValue("damage", card.Damage);
					    addPackageCmd.Parameters.AddWithValue("element_type", (int)card.ElementType);
					    addPackageCmd.Parameters.AddWithValue("monster_type", (int)card.MonsterType);
					    addPackageCmd.Parameters.AddWithValue("user_id", userId);
					    addPackageCmd.Parameters.AddWithValue("in_deck", false); 

					    addPackageCmd.Prepare();
					    if (addPackageCmd.ExecuteNonQuery() < 0) return false;
				    }
			    } catch (PostgresException) {
				    return false;
			    }
		    }

		    conn.Close();
			return true;
	    }

	    public CardStack GetMatchingCards(int userId, int element, int monster, int minDamage) {
		    var conn = dbConn.Connect();
		    CardStack cards = new CardStack();

		    string cmdText = "select * from \"card\" where user_id=@user_id and card_id not in (select offer.card_id from( select card_id from offer where user_id=@user_id_2) as offer) and in_deck=@in_deck and damage>=@min_damage";
		    if (element != -1)
			    cmdText += " and element_type=@element";
		    if (monster != -1)
			    cmdText += " and monster_type=@monster"; 

		    try {
			    using (var cardCmd = new NpgsqlCommand(cmdText ,conn)) {
				    cardCmd.Parameters.AddWithValue("user_id", userId);
				    cardCmd.Parameters.AddWithValue("user_id_2", userId);
				    cardCmd.Parameters.AddWithValue("in_deck", false);
				    cardCmd.Parameters.AddWithValue("min_damage", minDamage);
				    cardCmd.Parameters.AddWithValue("element", element);
				    cardCmd.Parameters.AddWithValue("monster", monster);
					cardCmd.Prepare();

					using (var reader = cardCmd.ExecuteReader()) {
						while (reader.Read()) {
							if ((int)reader["monster_type"] == 0) {
								// Spell
								Spell s = new Spell(
									(int)reader["card_id"],
									(string)reader["name"],
									(int)reader["damage"],
									(ElementType)reader["element_type"]);

								cards.AddCard(s);
							} else {
								// Monster
								Monster m = new Monster(
									(int)reader["card_id"],
									(string)reader["name"],
									(int)reader["damage"],
									(ElementType)reader["element_type"],
									(MonsterType)reader["monster_type"]);

								cards.AddCard(m);
							}
						}
					}
				}
		    } catch (PostgresException) {
			    return null; 
		    }

		    conn.Close();
		    return cards; 
	    }

	    public bool SwitchOwners(ICard card1, ICard card2) {
		    var conn = dbConn.Connect();

		    var user1 = GetCardOwner(card1.Id);
		    var user2 = GetCardOwner(card2.Id); 

			// Update Card1
			try {
				using (var updateCard1Cmd = new NpgsqlCommand("update \"card\" set user_id=@user_id where card_id=@card_id", conn)) {
					updateCard1Cmd.Parameters.AddWithValue("user_id", user2);
					updateCard1Cmd.Parameters.AddWithValue("card_id", card1.Id);
					updateCard1Cmd.Prepare();

					if (updateCard1Cmd.ExecuteNonQuery() < 0) return false;
				}
			} catch (PostgresException) {
				return false; 
			}

			// Update Card2
			try {
				using (var updateCard1Cmd = new NpgsqlCommand("update \"card\" set user_id=@user_id where card_id=@card_id", conn)) {
					updateCard1Cmd.Parameters.AddWithValue("user_id", user1);
					updateCard1Cmd.Parameters.AddWithValue("card_id", card2.Id);
					updateCard1Cmd.Prepare();

					if (updateCard1Cmd.ExecuteNonQuery() < 0) return false;
				}
			} catch (PostgresException) {
				return false;
			}

			conn.Close();
			return true; 
	    }

	    public bool SwitchOwnerAgainstCoins(IUser newOwner, ICard card) {
		    var conn = dbConn.Connect();

		    // Update Card
		    try {
			    using (var updateCardCmd = new NpgsqlCommand("update \"card\" set user_id=@user_id where card_id=@card_id", conn)) {
				    updateCardCmd.Parameters.AddWithValue("user_id", newOwner.Id);
				    updateCardCmd.Parameters.AddWithValue("card_id", card.Id);
				    updateCardCmd.Prepare();

				    if (updateCardCmd.ExecuteNonQuery() < 0) return false;
			    }
		    } catch (PostgresException) {
			    return false;
		    }

			conn.Close();
		    return true;
	    }
	}
}
