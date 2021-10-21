using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using monster_trading_card_game.CardCollections;
using monster_trading_card_game.Cards;
using monster_trading_card_game.Enums;

namespace monster_trading_card_game.Users {
    class User : IUser{
		// Constant Values
	    private const int NumberOfCoins = 20;
	    private const int EloStartingValue = 100;
	    private const int EloDecrement = 5;
	    private const int EloIncrement = 3;
	    private const int DefaultWinLoss = 0;
	    private const int MinDamage = 10; 
	    private const int MaxDamage = 100;
		private const int MinSpells = 35;
		private const int MaxSpells = 65;

		// Class properties
		public string Username { get; set; }
	    public string Password { get; set; }
	    public int Coins { get; set; }
	    public int Elo { get; set; }
	    public int Wins { get; set; }
	    public int Losses { get; set; }
	    public Stack CardStack { get; set; }
	    public Deck Deck { get; set; }

	    public User(string username, string password) {
		    Username = username;
		    Password = password;
		    Coins = NumberOfCoins;
		    Elo = EloStartingValue;
		    Wins = Losses = DefaultWinLoss;
		    CardStack = new Stack();
		    Deck = new Deck(); 
		}

	    public void AutoCreateDeck() {
		    var rand = new Random();

		    for (int i = 0; i < Deck.Capacity; i++) {
			    Deck.AddCard(CardStack.Cards.ElementAt(rand.Next(CardStack.Capacity)));
		    }
	    }

		public ICard ChooseRandomCard() {
			Random random = new Random();
			return Deck.Cards.ElementAt(random.Next(Deck.Count())); 
		}
		public void Challenge(IUser opponent) {
			// Create Battle Decks
			AutoCreateDeck();
			opponent.AutoCreateDeck();

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
		}

		public void LoseGame() {
			Losses++;
			Elo -= EloDecrement; 
		}

		public void GenerateCardStack() {
			var rand = new Random();
			int numberOfSpells = rand.Next(MinSpells, MaxSpells);
			int numberOfMonsters = CardStack.Capacity - numberOfSpells;

            foreach (var spell in GenerateRandomSpells(numberOfSpells).Cards) {
                CardStack.AddCard(spell);
            }

            foreach (var monster in GenerateRandomMonsters(numberOfMonsters).Cards) {
				CardStack.AddCard(monster);
			}
		}

		public Stack GenerateRandomSpells(int count) {
			Stack spellStack = new Stack(); 
			var rand = new Random();
			for (int i = 0; i < count; i++) {
				int damage = rand.Next(MinDamage, MaxDamage);
				ElementType element = (ElementType)rand.Next(Enum.GetNames(typeof(ElementType)).Length);
				string name = $"{element.ToString()} Spell"; 

				spellStack.AddCard(new Spell(name, damage, element));
			}
			return spellStack;
		}

		public Stack GenerateRandomMonsters(int count) {
			Stack monsterStack = new Stack();
			var rand = new Random();
			for (int i = 0; i < count; i++) {
				int damage = rand.Next(MinDamage, MaxDamage);
				ElementType element = (ElementType)rand.Next(Enum.GetNames(typeof(ElementType)).Length);
				MonsterType monster = (MonsterType)rand.Next(Enum.GetNames(typeof(MonsterType)).Length);
				string name = $"{element.ToString()} {monster.ToString()}";

				monsterStack.AddCard(new Monster(name, damage, element, monster));
			}
			return monsterStack;
		}
	}
}
