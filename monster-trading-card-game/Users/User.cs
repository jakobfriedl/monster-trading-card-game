using System;
using System.Collections.Generic;
using System.Text;
using monster_trading_card_game.CardCollections;

namespace monster_trading_card_game.Users {
    class User : IUser{
		// Constant Values
	    private const int NumberOfCoins = 20;
	    private const int EloStartingValue = 100;
	    private const int EloDecrement = 5;
	    private const int EloIncrement = 3;
	    private const int defaultWinLoss = 0; 

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
		    Wins = Losses = defaultWinLoss;
		}
    }
}
