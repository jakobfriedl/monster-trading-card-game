using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Linq;
using System.Threading;
using monster_trading_card_game.CardCollections;
using monster_trading_card_game.Cards;
using monster_trading_card_game.Database;
using monster_trading_card_game.Enums;
using Colorful;
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
	    private const int MaxDamage = 100;
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
			Console.WriteLine($"{Id} -- {Username}:{Password} - Coins: {Coins}");
		}

		public void BuyPackage(Package package) {
			Coins -= package.Cost;

			var dbUser = new DBUser();
			dbUser.BuyPackage(package, this);
		}
		public void OfferCard() {
			var dbCard = new DBCard();

			var cards = dbCard.GetCardStackFromUserId(Id);
			if (cards.Count() <= 0) {
				Console.WriteLine("You don't have cards to trade.\n", Color.Red);
				return; 
			}
			for (int i = 1; i <= cards.Count(); i++) {
				Console.Write($"  [{i}] ");
				cards.Cards[i - 1].PrintCardName();
				Console.WriteLine($" - {cards.Cards[i - 1].Damage}");
			}

			int cardId;
			while (true) {
				Console.Write("Enter ID of Card you want to offer (x to go back): ");

				try {
					var input = Console.ReadLine();
					if (input.ToLower() == "x") return;

					cardId = cards.Cards[Convert.ToInt32(input) - 1].Id;

					if (Id != dbCard.GetCardOwner(cardId)) {
						Console.Write("Invalid Card-ID. ", Color.Red);
					}

					break; 
				} catch (Exception) {
					Console.Write("Invalid Card-ID. ", Color.Red);
				}
			}

			int price;
			Console.Write("Enter an alternative price for the card (default: 5 coins): ");
			try {
				price = Convert.ToInt32(Console.ReadLine());
			} catch {
				price = 5;
			}

			var offer = new Offer(0, Id, cardId, price);

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
    }
}
