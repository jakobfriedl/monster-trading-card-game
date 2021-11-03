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
	    private const int InitialStackCapacity = 16; 
		private const int NumSpells = 8;
		private const int NumMonsters = 8;

		// Class properties

		public Guid id { get; set; }
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
			id = Guid.NewGuid();
		    Username = username;
		    Password = password;
		    Coins = NumberOfCoins;
		    Elo = EloStartingValue;
		    Wins = Losses = DefaultWinLoss;
		    CardStack = new CardStack();
			GenerateCardStack();
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
		}

		public void LoseGame() {
			Losses++;
			Elo -= EloDecrement; 
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
				string name = $"{element.ToString()} Spell"; 

				spellStack.AddCard(new Spell(name, damage, element));
			}
			return spellStack;
		}

		public CardStack GenerateRandomMonsters(int count) {
			CardStack monsterStack = new CardStack();
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

		public void Print() {
			Console.WriteLine($"[{id}] -- {Username}:{Password} - Coins: {Coins}, Cards: {CardStack.Count()}");
		}
	}
}
