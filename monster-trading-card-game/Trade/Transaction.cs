using System;
using System.Drawing;
using monster_trading_card_game.Database;
using Console = Colorful.Console; 

namespace monster_trading_card_game.Trade {
    public class Transaction {
		public int Id { get; set; }
	    public int User1 { get; set; }
	    public int User2 { get; set; }
	    public int Card1 { get; set; }
		public int Card2 { get; set; }
		public int Coins { get; set; }
		public int Timestamp { get; set; }

		// Full Constructor 
		public Transaction(int id, int user1, int user2, int card1, int card2, int coins, int timestamp) {
			Id = id;
			User1 = user1;
			User2 = user2;
			Card1 = card1;
			Card2 = card2;
			Coins = coins;
			Timestamp = timestamp; 
		}

		// Constructor for Trade where one user pays with coins
		public Transaction(int id, int user1, int user2, int card2, int coins, int timestamp) {
			Id = id;
			User1 = user1;
			User2 = user2;
			Card1 = -1;
			Card2 = card2; 
			Coins = coins;
			Timestamp = timestamp;
		}

		// Constructor for Package Purchase
		public Transaction(int id, int user1, int coins, int timestamp) {
			Id = id;
			User1 = user1;
			User2 = -1;
			Card1 = -1;
			Card2 = -1; 
			Coins = coins;
			Timestamp = timestamp; 
		}

		/// <summary>
		/// Print Transaction
		/// </summary>
		public void PrintTransaction() {
			var dbUser = new DBUser(); 
			var dbCard = new DBCard(); 

			var date = ConvertUnixTimeStampToDateTime(Timestamp);
			var user1 = dbUser.GetUsernameByUserId(User1); 

			// Package-Transaction
			if (User2 <= -1) {
				Console.Write($"{date}: ", Color.Gold);
				Console.WriteLine($"{user1} has bought a package for {Coins} coins.");
				return; 
			}
			
			var user2 = dbUser.GetUsernameByUserId(User2);
			var card2 = dbCard.GetCardByCardId(Card2);

			// Trade card against coins
			if (Card1 <= -1) {
				Console.Write($"{date}: ", Color.Gold);
				Console.Write($"{user1} has bought ");
				card2.PrintWithDamage();
				Console.WriteLine($" from {user2} for {Coins} coins.");
				return; 
			}

			var card1 = dbCard.GetCardByCardId(Card1);

			Console.Write($"{date}: ", Color.Gold);
			Console.Write($"{user1} has traded ");
			card1.PrintWithDamage();
			Console.Write(" for ");
			card2.PrintWithDamage();
			Console.WriteLine($" from {user2}.");
		}
		
		public static DateTime ConvertUnixTimeStampToDateTime(long unixTime) {
			DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0).ToLocalTime();
			return sTime.AddSeconds(unixTime);
		}
	}
}
