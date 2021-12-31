using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using monster_trading_card_game.CardCollections;
using monster_trading_card_game.Cards;
using monster_trading_card_game.Database;
using monster_trading_card_game.Enums;
using monster_trading_card_game.Trade;
using Console = Colorful.Console; 

namespace monster_trading_card_game.Users {
    class User : IUser{
		// Constant Values
	    private const int NumberOfCoins = 20;
	    private const int CoinIncrement = 1; 
	    private const int EloStartingValue = 100;
	    private const int EloDecrement = 5;
	    private const int EloIncrement = 3;
	    private const int DefaultWinLoss = 0;
	    private const int MinDamage = 50; 
	    private const int MaxDamage = 101;
		private const int NumSpells = 2;
		private const int NumMonsters = 2;

		// Class properties
		public int Id { get; set; }
		public string Username { get; set; }
	    public string Password { get; set; }
	    public int Coins { get; set; }
	    public int Elo { get; set; }
	    public int Wins { get; set; }
	    public int Losses { get; set; }
	    public CardStack CardStack { get; set; }
	    public Deck Deck { get; set; }

	    public User() {

	    }
	    public User(string username, string password) {
		    Id = 0; 
		    Username = username;
		    Password = password;
		    Coins = NumberOfCoins;
		    Elo = EloStartingValue;
		    Wins = Losses = DefaultWinLoss;
		    CardStack = new CardStack();
			GenerateCardStack();
		    Deck = new Deck(); 
			AutoCreateDeck();
		}

	    public User(int id, string username, string password, int coins, int elo, int wins, int losses) {
		    Id = id; 
		    Username = username;
		    Password = password;
		    Coins = coins;
		    Elo = elo;
		    Wins = wins;
		    Losses = losses;
		    CardStack = new CardStack();
		    Deck = new Deck(); 
	    }

	    public void AutoCreateDeck() {
		    // Random Cards
		    // var rand = new Random();
		    //for (int i = 0; i < Deck.Capacity; i++) {
		    //	Deck.AddCard(CardStack.Cards.ElementAt(rand.Next(CardStack.Count()-1)));
		    //}

		    // Strongest Cards
		    for (int i = 0; i < Deck.Capacity; i++) {
			    var card = CardStack.GetHighestDamageCard(); 
				Deck.AddCard(card);
				CardStack.RemoveCard(card);
		    }

		    foreach (var card in Deck.Cards) {
			    CardStack.AddCard(card);
		    }
	    }

	    public void BuildDeck() {
		    Console.WriteLine("Current Deck: ");
		    Deck.Print();
			
			Console.Write("Recreate Deck? [y|n]: ");
			if (Console.ReadLine().ToLower() != "y") {
				return;
			}
			Console.Clear();

			DBCard dbCard = new DBCard();
			var cards = dbCard.GetAllCardsFromUserId(Id);

			int j = 1;
			for (; j <= cards.Count(); j++) {
				Console.Write($"  [{j.ToString().PadLeft(2)}]  ");
				cards.Cards[j - 1].PrintCardName(); 
				Console.WriteLine($" - {cards.Cards[j - 1].Damage}");
			}

			Deck newDeck = new Deck(); 
			int i = 1; 
			while (i <= Deck.Capacity) {
				Console.Write($"Enter ID of Card #{i} (x to go back): ");
				int cardId;
				try {
					var input = Console.ReadLine();
					if (input.ToLower() == "x") return; 

					cardId = cards.Cards[Convert.ToInt32(input)-1].Id;
				} catch (Exception) {
					Console.WriteLine("Invalid Card-ID", Color.Red);
					continue; 
				}

				if (Id == dbCard.GetCardOwner(cardId)) {
					if (!newDeck.Cards.Any(c => c.Id == cardId)) {
						newDeck.AddCard(dbCard.GetCardByCardId(cardId));
					} else {
						Console.WriteLine("You cannot use the same card twice!", Color.Red);
						continue; 
					}
				} else {
					Console.WriteLine("Invalid Card-ID", Color.Red);
					continue; 
				}

				i++; 
			}

			if (dbCard.UpdateDeck(Id, newDeck))
				Deck = newDeck; 
	    }

		public ICard ChooseRandomCard() {
			Random random = new Random();
			return Deck.Cards.ElementAt(random.Next(Deck.Count()-1)); 
		}
		public void Challenge(IUser opponent) {
			Battle battle = new Battle(this, opponent);
			battle.StartBattle(); 
		}

		public void AddCardToStack(ICard card) {
			CardStack.AddCard(card);
		}

		public void AddCardToDeck(ICard card) {
			Deck.AddCard(card);
		}

		public void WinGame() {
			Wins++;
			Elo += EloIncrement;
			Coins += CoinIncrement;

			var dbUser = new DBUser();
			dbUser.UpdateStats(this); 
		}

		public void LoseGame() {
			Losses++;
			Elo -= EloDecrement;

			var dbUser = new DBUser();
			dbUser.UpdateStats(this);
		}

		public void GenerateCardStack() {
			//var rand = new Random();
			//int numberOfSpells = rand.Next(MinSpells, MaxSpells);
			//int numberOfMonsters = InitialStackCapacity - numberOfSpells;

            foreach (var spell in GenerateRandomSpells(NumSpells).Cards) {
                CardStack.AddCard(spell);
            }

            foreach (var monster in GenerateRandomMonsters(NumMonsters).Cards) {
				CardStack.AddCard(monster);
			}
		}

		public CardStack GenerateRandomSpells(int count) {
			CardStack spellStack = new CardStack(); 
			var rand = new Random();
			for (int i = 0; i < count; i++) {
				int damage = rand.Next(MinDamage, MaxDamage);
				ElementType element = (ElementType)rand.Next(Enum.GetNames(typeof(ElementType)).Length); 
				string name = $"{element} Spell"; 

				spellStack.AddCard(new Spell(0, name, damage, element));
			}
			return spellStack;
		}

		public CardStack GenerateRandomMonsters(int count) {
			CardStack monsterStack = new CardStack();
			var rand = new Random();
			for (int i = 0; i < count; i++) {
				int damage = rand.Next(MinDamage, MaxDamage);
				ElementType element = (ElementType)rand.Next(Enum.GetNames(typeof(ElementType)).Length);
				MonsterType monster = (MonsterType)rand.Next(Enum.GetNames(typeof(MonsterType)).Length)+1;
				string name = $"{element} {monster}";

				monsterStack.AddCard(new Monster(0, name, damage, element, monster));
			}
			return monsterStack;
		}

		public void Print() {
			var dbCard = new DBCard(); 

			Console.Write("Username: ", Color.Silver); Console.WriteLine(Username);
			Console.Write("Coins: ", Color.Silver); Console.WriteLine(Coins);
			Console.Write("Elo: ", Color.Gold); Console.WriteLine(Elo);
			Console.Write("Wins: ", Color.ForestGreen); Console.WriteLine(Wins);
			Console.Write("Losses: ", Color.Red); Console.WriteLine(Losses);
			double ratio = Losses == 0 ? 0 : (double)Wins / (double)Losses; // Calculate win-loss ratio
			Console.Write("W/L-Ratio: ", Color.Silver); Console.WriteLine(ratio);
			Console.Write("Cards: ", Color.Silver); Console.WriteLine(dbCard.GetAllCardsFromUserId(Id).Count() + "\n");
		}

		public void BuyPackage(Package package) {
			Coins -= package.Cost;

			var dbUser = new DBUser();
			dbUser.BuyPackage(package, this);
		}

		private Tuple<int, int, int> GetCardRequest() {
			Console.WriteLine("What should the element of the received card be?\nPress any other key if the element does not matter.");
			Console.Write("  [1] "); Console.WriteLine("Fire", Color.Firebrick);
			Console.Write("  [2] "); Console.WriteLine("Water", Color.DodgerBlue);
			Console.Write("  [3] "); Console.WriteLine("Normal", Color.Gray);

			// Enter Element-Type of requested card
			Console.Write(" >> ");
			int element = -1;
			try {
				var input = Console.ReadLine();
				if (input.ToLower() == "x") { Console.Clear(); return null; }

				element = Convert.ToInt32(input) - 1;
				if (element > 2 || element < 0) element = -1;
			} catch (FormatException) { }

			Console.WriteLine("What should the type of the received card be?\nPress any other key if the type does not matter.");
			Console.Write("  [1] "); Console.WriteLine("Spell", Color.DarkViolet);
			Console.Write("  [2] "); Console.WriteLine("Goblin", Color.Green);
			Console.Write("  [3] "); Console.WriteLine("Dragon", Color.Green);
			Console.Write("  [4] "); Console.WriteLine("Wizard", Color.Green);
			Console.Write("  [5] "); Console.WriteLine("Orc", Color.Green);
			Console.Write("  [6] "); Console.WriteLine("Knight", Color.Green);
			Console.Write("  [7] "); Console.WriteLine("Kraken", Color.Green);
			Console.Write("  [8] "); Console.WriteLine("Elf", Color.Green);

			// Enter Monster-Type of requested card
			Console.Write(" >> ");
			int monster = -1;
			try {
				var input = Console.ReadLine();
				if (input.ToLower() == "x") { Console.Clear(); return null; }

				monster = Convert.ToInt32(input) - 1;
				if (monster > 7 || monster < 0) monster = -1;
			} catch (FormatException) { }

			// Enter minimum damage of requested card
			int damage = 0;
			while (damage < 50 || damage > 100) {
				Console.Write("Enter the minimum damage of the requested card (50-100): ");
				try {
					var input = Console.ReadLine();
					if (input.ToLower() == "x") { Console.Clear(); return null; }

					damage = Convert.ToInt32(input);
				} catch (FormatException) { Console.Write("Invalid input. ", Color.Red); }
			}

			return new Tuple<int, int, int>(element, monster, damage); 
		}

		private int GetOfferPrice() {
			// Enter alternative price in coins
			int price = 0;
			while (price <= 0) {
				Console.Write("Enter a price for the card: ");
				try {
					var input = Console.ReadLine();
					if (input.ToLower() == "x") { Console.Clear(); return -1; }

					price = Convert.ToInt32(input);
				} catch (FormatException) { Console.Write("Invalid input. ", Color.Red); }
			}

			return price; 
		}

		public void OfferCard() {
			var dbCard = new DBCard();

			var cards = dbCard.GetCardStackFromUserId(Id);
			if (cards.IsEmpty()) {
				Console.WriteLine("You don't have cards to trade.\n", Color.Red);
				return; 
			}
			for (int i = 1; i <= cards.Count(); i++) {
				Console.Write($"  [{i.ToString().PadLeft(2)}] ");
				cards.Cards[i - 1].PrintCardName();
				Console.WriteLine($" - {cards.Cards[i - 1].Damage}");
			}

			int cardId;
			while (true) {
				Console.Write("Enter ID of Card you want to offer (x to go back): ");

				try {
					var input = Console.ReadLine();
					if (input.ToLower() == "x") { Console.Clear(); return; }

					cardId = cards.Cards[Convert.ToInt32(input) - 1].Id;

					if (Id != dbCard.GetCardOwner(cardId)) {
						Console.Write("Invalid Card-ID. ", Color.Red);
					}

					break; 
				} catch (Exception) {
					Console.Write("Invalid Card-ID. ", Color.Red);
				}
			}

			Console.Write("You chose: ");
			var card = dbCard.GetCardByCardId(cardId);
			card.PrintCardName();
			Console.WriteLine($" -   {card.Damage}");

			// Get requested Card
			var request = GetCardRequest(); // item1...element, item2...monster, item3...min_damage
			if (request == null) return; 
			// Get alternative Price
			var price = GetOfferPrice();
			if (price <= -1) return;

			var offer = new Offer(0, Id, cardId, request.Item1, request.Item2, request.Item3, price);

			var dbOffer = new DBOffer();
			if (dbOffer.CreateNewOffer(offer)) {
				Console.WriteLine("Offer successful!", Color.ForestGreen);
				Thread.Sleep(1000);
				Console.Clear();
			} else {
				Console.WriteLine("Offer failed!", Color.Red);
				Thread.Sleep(1000);
				Console.Clear();
			}
		}

		private List<Offer> ListOffers(bool own) {
			var dbOffer = new DBOffer();
			
			var offers = own ? dbOffer.GetOffersByUserId(Id) : dbOffer.GetOffersFromOtherUsers(Id);

			if (offers.Count <= 0) {
				Console.WriteLine("No offers available.\n", Color.Red);
				return null;
			}

			// Table Heading
			Console.Write("    Offer".PadRight(own ? 29 : 44), Color.Gold);
			Console.WriteLine("Request", Color.Gold);

			if(own)
				Console.WriteLine($"{"#".PadRight(4)}{"Card Name".PadRight(15)}{"Damage".PadRight(10)}{"Element".PadRight(9)}{"Card-Type".PadRight(12)}{"Min-Damage".PadRight(12)}{"Price".PadRight(7)}", Color.Silver);
			else 
				Console.WriteLine($"{"#".PadRight(4)}{"Username".PadRight(15)}{"Card Name".PadRight(15)}{"Damage".PadRight(10)}{"Element".PadRight(9)}{"Card-Type".PadRight(12)}{"Min-Damage".PadRight(12)}{"Price".PadRight(7)}", Color.Silver);

			int i = 1;
			foreach (var offer in offers) {
				Console.Write(i.ToString().PadRight(4));
				if(own)
					offer.PrintOwn();
				else 
					offer.PrintOther();
				i++;
			}
			
			return offers; 
		}

		private Offer SelectOffer(List<Offer> offers) {
			Offer offer = null;
			string input = "";

			while (offer == null) {
				Console.Write("\nEnter # of the offer you want to select (x to go back): ");
				input = Console.ReadLine();
				if (input.ToLower() == "x") {
					Console.Clear();
					return null;
				}

				try {
					offer = offers.ElementAt(Convert.ToInt32(input) - 1);
				} catch (Exception) {
					Console.Write("Invalid Input. ", Color.Red);
				}
			}
			Console.WriteLine($"You selected offer #{input}");

			return offer; 
		}

		public void ManageOwnOffers() {
			var dbOffer = new DBOffer();
			
			var offers = ListOffers(true);
			if (offers == null) return;
			var selectedOffer = SelectOffer(offers);
			if (selectedOffer == null) return;

			Console.WriteLine("\nWhat would you like to do?");
			Console.WriteLine("  [1] Remove Offer");
			Console.WriteLine("  [2] Change Request");
			Console.WriteLine("  [3] Change Price");
			Console.WriteLine("  [X] Go back");

			while (true) {
				Console.Write(" >> ");
				switch (Console.ReadLine().ToUpper()) {
					case "1":
						if (dbOffer.RemoveOfferByOfferId(selectedOffer.Id)) {
							Console.WriteLine("Offer successfully removed." ,Color.ForestGreen);
						}
						break;
					case "2":
						var request = GetCardRequest();
						if (request == null) return;

						var changedRequest = new Offer(0, Id, selectedOffer.CardId, request.Item1, request.Item2, request.Item3, selectedOffer.Price);

						if (dbOffer.UpdateOffer(selectedOffer.Id, changedRequest)) {
							Console.WriteLine("Offer successfully updated.", Color.ForestGreen);
							return;
						}
						break;
					case "3":
						var price = GetOfferPrice();
						if (price <= -1) return;

						var changedPrice = new Offer(0, Id, selectedOffer.CardId, selectedOffer.Element, selectedOffer.Monster, selectedOffer.MinDamage, price);

						if (dbOffer.UpdateOffer(selectedOffer.Id, changedPrice)) {
							Console.WriteLine("Offer successfully updated.", Color.ForestGreen);
							return;
						}
						break;
					case "X":
						Console.Clear();
						return;
					default: 
						Console.Write("Invalid input.", Color.Red);
						break; 
				}
			}
		}

		public void FindOtherOffers() {
			var dbOffer = new DBOffer();
			var dbCard = new DBCard();
			var dbUser = new DBUser();

			var offers = ListOffers(false);
			if (offers == null) return;
			var selectedOffer = SelectOffer(offers);
			if (selectedOffer == null) return;

			// Choose Payment-Method
			Console.WriteLine("\nHow do you want to acquire this card?");
			Console.WriteLine("  [1] Trade Cards");
			Console.WriteLine("  [2] Buy with Coins");
			Console.WriteLine("  [X] Go back");

			// Trade
			bool transactionCompleted = false;
			while (!transactionCompleted) {
				Console.Write(" >> ");
				switch (Console.ReadLine().ToUpper()) {
					case "1":
						// Select a card matching the trade request from your stack
						var matchingCards = dbCard.GetMatchingCards(Id, selectedOffer.Element, selectedOffer.Monster, selectedOffer.MinDamage);

						if (matchingCards.IsEmpty()) {
							Console.WriteLine("No matching cards found!", Color.Red);
							break;
						}

						// Print cards that can be traded
						int i = 1; 
						foreach (var card in matchingCards.Cards) {
							Console.Write($"  [{i.ToString().PadLeft(2)}] ");
							card.PrintCardName();
							Console.WriteLine($"-  {card.Damage}");
							i++; 
						}

						// Get Card which will be traded
						while (!transactionCompleted) {
							Console.Write("Which card do you want to trade (x to go back)? ");
							var input = Console.ReadLine();
							if (input.ToLower() == "x") break;

							try {
								var cardToTrade = matchingCards.Cards.ElementAt(Convert.ToInt32(input) - 1);

								// Remove Card from Stack of logged in user
								CardStack.RemoveCard(cardToTrade);
								
								// Switch Owners of the two cards and remove offer
								if (dbCard.SwitchOwners(cardToTrade, dbCard.GetCardByCardId(selectedOffer.CardId)) && dbOffer.RemoveOfferByOfferId(selectedOffer.Id)) {
									Console.WriteLine("Trade successful", Color.ForestGreen);
									transactionCompleted = true;
								} 

							} catch (Exception e) {
								System.Console.WriteLine(e);
								Console.Write("Invalid input. ", Color.Red);
							}
						}

						break;
					case "2":
						// Pay the provided price in coins
						if (selectedOffer.Price > Coins) {
							Console.WriteLine("You don't have enough coins to purchase this card!", Color.Red);
							break;
						}

						Coins -= selectedOffer.Price; // Update coins of current User

						// Switch Owners remove offer and transfer coins from buyer to seller of the card
						if (dbUser.TransferCoins(this, selectedOffer.UserId, selectedOffer.Price) &&
						    dbCard.SwitchOwnerAgainstCoins(this, dbCard.GetCardByCardId(selectedOffer.CardId)) && dbOffer.RemoveOfferByOfferId(selectedOffer.Id)) {
							Console.WriteLine("Trade successful", Color.ForestGreen);
							transactionCompleted = true;
						}

						break;
					case "X":
						Console.Clear();
						transactionCompleted = true;
						break;
					default:
						Console.Write("Invalid input.", Color.Red);
						break;
				}
			}
		}
    }
}
