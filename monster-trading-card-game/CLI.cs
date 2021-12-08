using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Xml;
using monster_trading_card_game.CardCollections;
using monster_trading_card_game.Database;
using monster_trading_card_game.Enums;
using monster_trading_card_game.Users;

namespace monster_trading_card_game {
    class CLI {
	    public bool IsLoggedIn { get; set; } 
		public IUser LoggedInUser { get; set; }

        public CLI() {
	        IsLoggedIn = false; 
        }

        public Command GetUserCommand() {
	        if(!IsLoggedIn)
				Console.WriteLine("[ REGISTER | LOGIN | QUIT ]");
            else
				Console.WriteLine("[ DECK | BATTLE | TRADE | BUY | SCORES | PROFILE | LOGOUT | QUIT ]");

	        Console.Write(" >> ");
	        string command = Console.ReadLine(); 

	        if (!IsLoggedIn) {
		        switch (command.ToUpper()) {
			        case "REGISTER":
				        return Command.Register;
					case "LOGIN": 
				        return Command.Login;
			        case "QUIT":
				        return Command.Quit;
			        default:
				        Console.WriteLine("Invalid Command");
				        break;
				}
	        } else {
		        switch (command.ToUpper()) {
			        case "DECK":
						return Command.Deck;
					case "BATTLE":
						return Command.Battle;
					case "TRADE":
						return Command.Trade;
					case "BUY":
						return Command.Buy;
					case "SCORES":
						return Command.Scores;
					case "PROFILE":
						return Command.Profile;
			        case "LOGOUT":
						return Command.Logout;
					case "QUIT":
						return Command.Quit;
					default:
						Console.WriteLine("Invalid Command");
						break;
		        }
			}

	        return Command.Invalid; 
        }

        public bool RegisterUser() {
	        Console.Write("Username: ");
	        string username = Console.ReadLine();
	        Console.Write("Password: ");
	        string password = Console.ReadLine();

	        DBUser db = new DBUser();
	        return db.RegisterUser(new User(username, password));
        }

        public bool LoginUser() {
			Console.Write("Username: ");
			string username = Console.ReadLine();
			Console.Write("Password: ");
			string password = Console.ReadLine();

			DBUser db = new DBUser(); 
			
			LoggedInUser = db.LoginUser(username, password);
			if (LoggedInUser != null) {
				IsLoggedIn = true;
				return true; 
			} 
			IsLoggedIn = false;
			return false; 
        }

        public void LogoutUser() {
	        IsLoggedIn = false;
	        LoggedInUser = null; 
        }

        public void Battle() {
			IUser bot = new User("Bot", "da39a3ee5e6b4b0d3255bfef95601890afd80709");
			LoggedInUser.Challenge(bot);

			var dbCard = new DBCard();
			LoggedInUser.Deck = dbCard.GetDeckFromUserId(LoggedInUser.Id); 
        }

        public void Profile() {
	        Console.WriteLine($"Profile of {LoggedInUser.Username}:");
	        Console.WriteLine($"Coins: {LoggedInUser.Coins}");
	        Console.WriteLine($"Elo: {LoggedInUser.Elo}");
	        Console.WriteLine($"Wins: {LoggedInUser.Wins}");
	        Console.WriteLine($"Losses: {LoggedInUser.Losses}");
	        var dbCard = new DBCard();
	        Console.WriteLine($"Cards: {dbCard.GetAllCardsFromUserId(LoggedInUser.Id).Count()}");
	        Console.WriteLine("Deck:");
	        dbCard.GetDeckFromUserId(LoggedInUser.Id).Print();
	        Console.WriteLine("Unused Cards:");
			dbCard.GetCardStackFromUserId(LoggedInUser.Id).Print();

        }

        public void GetPackage() {
			Console.Clear();
	        Console.WriteLine($"Choose a package to buy (5 Coins): \n" +
	                          "		[1] Fire Package \n" +
	                          "		[2] Water Package \n" +
	                          "		[3] Normal Package \n" +
	                          "		[4] Monster Package \n" +
	                          "		[5] Spell Package");
	        Console.Write("Package Number (x to go back): ");
	        var package = Console.ReadLine();

	        if (package == "x") return;

	        Package p = new Package((PackageType)(Convert.ToInt32(package)-1));

	        if (LoggedInUser.Coins < p.Cost) {
		        Console.WriteLine("You don't have enough coins to purchase this package.");
		        return; 
	        }

	        LoggedInUser.BuyPackage(p);
        }
    }
}
