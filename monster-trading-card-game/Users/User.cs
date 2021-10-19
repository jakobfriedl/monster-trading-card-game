using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using monster_trading_card_game.CardCollections;
using monster_trading_card_game.Cards;

namespace monster_trading_card_game.Users {
    class User : IUser{
		// Constant Values
	    private const int NumberOfCoins = 20;
	    private const int EloStartingValue = 100;
	    private const int EloDecrement = 5;
	    private const int EloIncrement = 3;
	    private const int DefaultWinLoss = 0; 

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

		public ICard ChooseRandomCard() {
			Random random = new Random();
			return CardStack.Cards.ElementAt(random.Next(CardStack.Count())); 
		}
		public void Challenge(IUser opponent) {
			Battle battle = new Battle(this, opponent);
			battle.StartBattle(); 
		}

		public void AddCardToStack(ICard card) {
			CardStack.AddCard(card);
		}

		public void WinGame() {
			Wins++;
			Elo += EloIncrement;
		}

		public void LoseGame() {
			Losses++;
			Elo -= EloDecrement; 
		}
	}
}
