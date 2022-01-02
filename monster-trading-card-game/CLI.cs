using System;
using System.Drawing;
using System.Threading;
using monster_trading_card_game.CardCollections;
using monster_trading_card_game.Database;
using monster_trading_card_game.Enums;
using monster_trading_card_game.Users;
using Console = Colorful.Console;  

namespace monster_trading_card_game {
    class CLI {
	    public bool IsLoggedIn { get; set; } 
		public IUser LoggedInUser { get; set; }

        public CLI() {
	        IsLoggedIn = false; 
        }

		/// <summary>
		/// Shows the main menu and reads the input from the user
		/// </summary>
		/// <returns> Command that determines action </returns>
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

		/// <summary>
		/// Obfuscates password on input
		/// </summary>
		/// <returns> Password </returns>
        private string ReadPassword() {
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

	        return password;
        }

		/// <summary>
		/// Reads registration data from user and registers user in Database
		/// </summary>
		/// <returns> true if registration successful </returns>
		/// <returns> false if registration fails </returns>
        public bool RegisterUser() {
	        Console.Write("Username: ", Color.Silver);
	        string username = Console.ReadLine();
	        Console.Write("Password: ", Color.Silver);

	        var password = ReadPassword();

			DBUser db = new DBUser();
	        return db.RegisterUser(new User(username, password));
        }

		/// <summary>
		/// Reads login data from user and tries to log user in
		/// </summary>
		/// <returns> true if login successful </returns>
		/// <returns> false if login fails </returns>
		public bool LoginUser() {
			Console.Write("Username: ", Color.Silver);
			string username = Console.ReadLine();
			Console.Write("Password: ", Color.Silver);

			var password = ReadPassword();

			DBUser db = new DBUser(); 
			
			LoggedInUser = db.LoginUser(username, password);
			if (LoggedInUser != null) {
				IsLoggedIn = true;
				return true; 
			} 
			IsLoggedIn = false;
			return false; 
        }

		/// <summary>
		/// Logs out user by unsetting variables
		/// </summary>
        public void LogoutUser() {
	        IsLoggedIn = false;
	        LoggedInUser = null; 
        }

		/// <summary>
		/// Shows menu for Battle command and gets user input 
		/// </summary>
        public void Battle() {
	        bool battleFinished = false; 
	        string action = "";

	        while (action != "X" && !battleFinished) {
		        Console.Write("  [1] "); Console.WriteLine("Play against Bot");
		        Console.Write("  [2] "); Console.WriteLine("Send Battle Request");
		        Console.Write("  [3] "); Console.WriteLine("Accept/Deny Battle Request");
		        Console.Write("  [X] "); Console.WriteLine("Leave Battle Area");

		        Console.Write(" >> ");
		        action = Console.ReadLine().ToUpper();

		        switch (action) {
			        case "1":
				        Console.Clear();
				        IUser bot = new User("Bot", "da39a3ee5e6b4b0d3255bfef95601890afd80709");
				        LoggedInUser.Challenge(bot);
				        battleFinished = true;
				        break;
					case "2":
				        Console.Clear();
				        LoggedInUser.SendBattleRequest(); 
						break;
					case "3":
						Console.Clear();
						LoggedInUser.HandleBattleRequests();
						break;
			        case "X":
				        return;
			        default:
				        Console.Clear();
				        Console.WriteLine("Invalid input.\n", Color.Red);
				        break;
		        }
	        }

			var dbCard = new DBCard();
			LoggedInUser.Deck = dbCard.GetDeckFromUserId(LoggedInUser.Id);
			Console.WriteLine("\nPress any key to continue.");
			Console.ReadKey(); 
        }

		/// <summary>
		/// Shows menu for Profile command and gets user input
		/// </summary>
        public void Profile() {
	        string action = ""; 
			while(action != "X"){

				var dbCard = new DBCard();

				Console.Write("  [1] "); Console.WriteLine("Show Profile Information");
				Console.Write("  [2] "); Console.WriteLine("Show Deck");
				Console.Write("  [3] "); Console.WriteLine("Show Usable Cards");
				Console.Write("  [4] "); Console.WriteLine("Change Password");
				Console.Write("  [5] "); Console.WriteLine("Show Transaction History");
				Console.Write("  [X] "); Console.WriteLine("Leave Profile Settings");

				Console.Write(" >> ");
				action = Console.ReadLine().ToUpper();

				switch (action) {
					case "1":
						Console.Clear();
						LoggedInUser.Print();
						break;
					case "2":
						Console.Clear();
						dbCard.GetDeckFromUserId(LoggedInUser.Id).Print();
						Console.WriteLine();
						break;
					case "3":
						Console.Clear();
						dbCard.GetAllCardsFromUserId(LoggedInUser.Id).Print();
						Console.WriteLine();
						break;
					case "4":
						Console.Clear();
						Console.Write("Enter old password: ", Color.Silver);
						string oldPassword = ReadPassword();
						Console.Write("\nEnter new password: ", Color.Silver);
						string newPassword = ReadPassword();
						Console.Write("\nRepeat new password: ", Color.Silver);
						string repeatPassword = ReadPassword();

						var dbUser = new DBUser();
						if (dbUser.ChangePassword(LoggedInUser.Id, oldPassword, newPassword, repeatPassword)) {
							Console.WriteLine("\nPassword successfully changed.", Color.ForestGreen);
						} else {
							Console.WriteLine("\nPassword change failed.", Color.Red);
						}
						break;
					case "5":
						Console.Clear();
						LoggedInUser.ShowTransactions(); 
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

		/// <summary>
		/// Shows menu for Buy command and gets user input for package number
		/// </summary>
		public void GetPackage() {
			Console.WriteLine("\nChoose a package to buy (5 Coins):");
			Console.Write("  [1] "); Console.WriteLine("Fire Package", Color.Firebrick);
			Console.Write("  [2] "); Console.WriteLine("Water Package", Color.DodgerBlue);
			Console.Write("  [3] "); Console.WriteLine("Normal Package", Color.Gray);
			Console.Write("  [4] "); Console.WriteLine("Monster Package", Color.Green);
			Console.Write("  [5] "); Console.WriteLine("Spell Package", Color.DarkViolet);
			Console.Write("  [X] "); Console.WriteLine("Leave Card Shop");

			string package;
			while (true) {
				Console.Write(" >> ");
				package = Console.ReadLine();
				
				if (package == "x") return;

				try {
					if (Convert.ToInt32(package) <= 5 && Convert.ToInt32(package) >= 1) {
						break;
					}
					Console.Write("Invalid input.", Color.Red);
				} catch (FormatException) {
					Console.Write("Invalid input.", Color.Red);
				}
			}

			Package p = new Package((PackageType)(Convert.ToInt32(package)-1));

	        if (LoggedInUser.Coins < p.Cost) {
		        Console.WriteLine("You don't have enough coins to purchase this package.", Color.Red);
				Thread.Sleep(1000);
		        return; 
	        }
	        LoggedInUser.BuyPackage(p);
        }

		/// <summary>
		/// Shows Scoreboard for Score command
		/// </summary>
		public void Scores() {
			var dbUser = new DBUser();

			// Table Heading
			Console.Clear(); 
			Console.WriteLine($"{"#".PadRight(4)}{"Username".PadRight(20)}{"Elo".PadRight(10)}{"Wins".PadRight(10)}{"Losses".PadRight(10)}W/L-Ratio", Color.Silver);

			int i = 1; 
			foreach (var user in dbUser.GetAllUsers()) {
				// Display Scores of each User
				double ratio = user.Losses == 0 ? 0 : (double)user.Wins / (double)user.Losses; // Calculate win-loss ratio

				Console.WriteLine(
					$"{i.ToString().PadRight(4)}{user.Username.PadRight(20)}{user.Elo.ToString().PadRight(10)}{user.Wins.ToString().PadRight(10)}{user.Losses.ToString().PadRight(10)}{ratio.ToString()}", 
					user.Id == LoggedInUser.Id ? Color.Gold : Color.White);
				i++; 
			}

			System.Console.WriteLine("\nPress any key to continue.");
			Console.ReadKey();
		}

		/// <summary>
		/// Shows menu for Trade command and gets user input
		/// </summary>
		public void Trade() {
			string action = "";

			while (action != "X") {
				Console.Write("  [1] "); Console.WriteLine("Offer Card");
				Console.Write("  [2] "); Console.WriteLine("Manage Offers");
				Console.Write("  [3] "); Console.WriteLine("Find Cards");
				Console.Write("  [X] "); Console.WriteLine("Leave Trading Hall");

				Console.Write(" >> ");
				action = Console.ReadLine().ToUpper();

				switch (action) {
					case "1":
						// Offer a new card
						Console.Clear();
						LoggedInUser.OfferCard();
						break;
					case "2":
						// Show and edit your own offers
						Console.Clear();
						LoggedInUser.ManageOwnOffers();
						break;
					case "3":
						// Show offers of other users but not your own
						Console.Clear();
						LoggedInUser.FindOtherOffers();
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
    }
}
