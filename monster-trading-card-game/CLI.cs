using System;
using System.Drawing;
using System.Threading;
using monster_trading_card_game.CardCollections;
using monster_trading_card_game.Database;
using monster_trading_card_game.Enums;
using monster_trading_card_game.Users;
using Colorful;
using Console = Colorful.Console;  

namespace monster_trading_card_game {
    class CLI {
	    public bool IsLoggedIn { get; set; } 
		public IUser LoggedInUser { get; set; }

        public CLI() {
	        IsLoggedIn = false; 
        }

        public Command GetUserCommand() {
			Console.Clear();
	        if(!IsLoggedIn)
				Console.WriteLine("[ REGISTER | LOGIN | QUIT ]", Color.Silver);
            else
				Console.WriteLine("[ DECK | BATTLE | TRADE | BUY | SCORES | PROFILE | LOGOUT | QUIT ]", Color.Silver);

	        Console.Write(" >> ", Color.Silver);
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
	        Console.Write("Username: ", Color.Silver);
	        string username = Console.ReadLine();
	        Console.Write("Password: ", Color.Silver);
			var password = string.Empty;
			ConsoleKey key;
			do {
				var keyInfo = Console.ReadKey(true);
				key = keyInfo.Key;

				if (key == ConsoleKey.Backspace && password.Length > 0) {
					Console.Write("\b \b");
					password = password[0..^1];
				} else if (!char.IsControl(keyInfo.KeyChar)) {
					Console.Write("*");
					password += keyInfo.KeyChar;
				}
			} while (key != ConsoleKey.Enter);

			DBUser db = new DBUser();
	        return db.RegisterUser(new User(username, password));
        }

        public bool LoginUser() {
			Console.Write("Username: ", Color.Silver);
			string username = Console.ReadLine();
			Console.Write("Password: ", Color.Silver);

			var password = string.Empty;
			ConsoleKey key;
			do {
				var keyInfo = Console.ReadKey(true);
				key = keyInfo.Key;

				if (key == ConsoleKey.Backspace && password.Length > 0) {
					Console.Write("\b \b");
					password = password[0..^1];
				} else if (!char.IsControl(keyInfo.KeyChar)) {
					Console.Write("*");
					password += keyInfo.KeyChar;
				}
			} while (key != ConsoleKey.Enter);

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
	        string action = ""; 
			while(action != "X"){

				var dbCard = new DBCard();

				Console.Write("  [I] "); Console.WriteLine("Show Profile Information");
				Console.Write("  [D] "); Console.WriteLine("Show Deck");
				Console.Write("  [C] "); Console.WriteLine("Show All Cards");
				Console.Write("  [P] "); Console.WriteLine("Change Password");
				Console.Write("  [X] "); Console.WriteLine("Leave Profile Settings");

				Console.Write(">> ");
				action = Console.ReadLine().ToUpper();

				switch (action) {
					case "I":
						Console.Clear();
						Console.Write("Username: ", Color.Silver);
						Console.WriteLine(LoggedInUser.Username);
						Console.Write("Coins: ", Color.Silver);
						Console.WriteLine(LoggedInUser.Coins);
						Console.Write("Elo: ", Color.Silver);
						Console.WriteLine(LoggedInUser.Elo);
						Console.Write("Wins: ", Color.ForestGreen);
						Console.WriteLine(LoggedInUser.Wins);
						Console.Write("Losses: ", Color.Red);
						Console.WriteLine(LoggedInUser.Losses);

						Console.Write("Cards: ", Color.Silver);
						Console.WriteLine(dbCard.GetAllCardsFromUserId(LoggedInUser.Id).Count() + "\n");
						break;
					case "D":
						Console.Clear();
						dbCard.GetDeckFromUserId(LoggedInUser.Id).Print();
						Console.WriteLine();
						break;
					case "C":
						Console.Clear();
						dbCard.GetAllCardsFromUserId(LoggedInUser.Id).Print();
						Console.WriteLine();
						break;
					case "P":
						Console.Clear(); 
						break;
					case "X":
						return;
					default:
						Console.Clear();
						Console.WriteLine("Invalid input.\n", Color.Red);
						break;
				}
			}
        }

		public void GetPackage() {
			Console.WriteLine($"Choose a package to buy (5 Coins):");
			Console.Write("  [1] "); Console.WriteLine("Fire Package", Color.Firebrick);
			Console.Write("  [2] "); Console.WriteLine("Water Package", Color.DodgerBlue);
			Console.Write("  [3] "); Console.WriteLine("Normal Package", Color.Gray);
			Console.Write("  [4] "); Console.WriteLine("Monster Package", Color.Green);
			Console.Write("  [5] "); Console.WriteLine("Spell Package", Color.DarkViolet);

			Console.Write("Package Number (x to go back): ");
	        var package = Console.ReadLine();

	        if (package == "x") return;

	        Package p = new Package((PackageType)(Convert.ToInt32(package)-1));

	        if (LoggedInUser.Coins < p.Cost) {
		        Console.WriteLine("You don't have enough coins to purchase this package.", Color.Red);
				Thread.Sleep(1000);
		        return; 
	        }
	        LoggedInUser.BuyPackage(p);
        }
    }
}
