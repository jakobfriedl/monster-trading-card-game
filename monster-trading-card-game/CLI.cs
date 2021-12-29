using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
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

        public bool RegisterUser() {
	        Console.Write("Username: ", Color.Silver);
	        string username = Console.ReadLine();
	        Console.Write("Password: ", Color.Silver);

	        var password = ReadPassword();

			DBUser db = new DBUser();
	        return db.RegisterUser(new User(username, password));
        }

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

        public void LogoutUser() {
	        IsLoggedIn = false;
	        LoggedInUser = null; 
        }

        public void Battle() {
	        bool battleFinished = false; 
	        string action = "";

	        while (action != "X" && !battleFinished) {
		        Console.Write("  [1] "); Console.WriteLine("Play against Bot");
		        Console.Write("  [2] "); Console.WriteLine("Play against other Player");
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

						// TODO: Send Battle Request to other players or accept battle requests from other players

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

        public void Profile() {
	        string action = ""; 
			while(action != "X"){

				var dbCard = new DBCard();

				Console.Write("  [1] "); Console.WriteLine("Show Profile Information");
				Console.Write("  [2] "); Console.WriteLine("Show Deck");
				Console.Write("  [3] "); Console.WriteLine("Show All Cards");
				Console.Write("  [4] "); Console.WriteLine("Change Password");
				Console.Write("  [X] "); Console.WriteLine("Leave Profile Settings");

				Console.Write(" >> ");
				action = Console.ReadLine().ToUpper();

				switch (action) {
					case "1":
						Console.Clear();
						Console.Write("Username: ", Color.Silver); Console.WriteLine(LoggedInUser.Username);
						Console.Write("Coins: ", Color.Silver); Console.WriteLine(LoggedInUser.Coins); 
						Console.Write("Elo: ", Color.Gold); Console.WriteLine(LoggedInUser.Elo);
						Console.Write("Wins: ", Color.ForestGreen); Console.WriteLine(LoggedInUser.Wins);
						Console.Write("Losses: ", Color.Red); Console.WriteLine(LoggedInUser.Losses);
						Console.Write("Cards: ", Color.Silver); Console.WriteLine(dbCard.GetAllCardsFromUserId(LoggedInUser.Id).Count() + "\n");

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

						// TODO: Change Password

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
					Console.Write("Invalid input. ", Color.Red);
				} catch (FormatException) {
					Console.Write("Invalid input. ", Color.Red);
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

		public void Scores() {
			var dbUser = new DBUser();

			var users = dbUser.GetAllUsers();

			// Table Heading
			Console.WriteLine($"\n{"#".PadRight(4)}{"Username".PadRight(20)}{"Elo".PadRight(10)}{"Wins".PadRight(10)}{"Losses".PadRight(10)}", Color.Silver);

			int i = 1; 
			foreach (var user in users) {
				// Display Scores of each User
				Console.WriteLine($"{i.ToString().PadRight(4)}{user.Item1.PadRight(20)}{user.Item2.ToString().PadRight(10)}{user.Item3.ToString().PadRight(10)}{user.Item4.ToString().PadRight(10)}", user.Item1 == LoggedInUser.Username ? Color.Gold : Color.White);
				i++; 
			}

			System.Console.WriteLine("\nPress any key to continue.");
			Console.ReadKey();
		}

		public void Trade() {
			var dbOffer = new DBOffer(); 
			var dbCard = new DBCard();
			var dbUser = new DBUser(); 

			string action = "";

			while (action != "X") {
				Console.Write("  [1] "); Console.WriteLine("Offer Card");
				Console.Write("  [2] "); Console.WriteLine("Manage Offers");
				Console.Write("  [3] "); Console.WriteLine("Check Trade Requests");
				Console.Write("  [4] "); Console.WriteLine("Check Card Offers");
				Console.Write("  [X] "); Console.WriteLine("Leave Trading Hall");

				Console.Write(" >> ");
				action = Console.ReadLine().ToUpper();

				switch (action) {
					case "1":
						Console.Clear();
						LoggedInUser.OfferCard();
						break;
					case "2":
						Console.Clear();
						var ownOffers = dbOffer.GetOffersByUserId(LoggedInUser.Id);

						// Table Heading
						Console.WriteLine($"{"#".PadRight(4)}{"Card Name".PadRight(15)}{"Damage".PadRight(10)}{"Price".PadRight(7)}", Color.Silver);
						
						int i = 1; 
						foreach (var offer in ownOffers) {
							// List all Offers of a specific User
							var card = dbCard.GetCardByCardId(offer.CardId);
							Console.Write(i.ToString().PadRight(4));
							card.PrintCardName();
							Console.WriteLine($"{card.Damage.ToString().PadRight(10)}{offer.Price.ToString().PadRight(7)}");
							i++; 
						}

						// TODO: Remove Offer or Edit Price from Offer

						break;
					case "3":
						Console.Clear();

						// TODO: Show who offers cards/coins for your offers
						// TODO: Accept or deny trade requests with cards

						break;
					case "4":
						// Show offers of other users but not your own
						Console.Clear();
						var otherOffers = dbOffer.GetOffersFromOtherUsers(LoggedInUser.Id);

						// Table Heading
						Console.WriteLine($"{"#".PadRight(4)}{"Username".PadRight(15)}{"Card Name".PadRight(15)}{"Damage".PadRight(10)}{"Price".PadRight(7)}", Color.Silver);

						int j = 1;

						if (otherOffers.Count <= 0) {
							Console.WriteLine("No offers available.\n", Color.Red);
						}

						foreach (var offer in otherOffers) {
							// List all Offers that don't belong to the current User
							var card = dbCard.GetCardByCardId(offer.CardId);
							var user = dbUser.GetUsernameByUserId(offer.UserId); 

							Console.Write(j.ToString().PadRight(4));
							Console.Write(user.PadRight(15));
							card.PrintCardName();
							Console.WriteLine($"{card.Damage.ToString().PadRight(10)}{offer.Price.ToString().PadRight(7)}");
							j++;
						}

						// TODO: Select an offer and either pay the coins or choose a card to trade

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
