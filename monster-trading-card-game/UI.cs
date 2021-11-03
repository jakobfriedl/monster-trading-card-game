using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using monster_trading_card_game.Enums;
using monster_trading_card_game.Users;

namespace monster_trading_card_game {
    class UI {
	    public bool IsLoggedIn { get; set; } 
		public IUser loggedInUser { get; set; }

        public UI() {
	        IsLoggedIn = false; 
        }

        public Command GetUserCommand() {
	        if(!IsLoggedIn)
				Console.WriteLine("[ REGISTER | LOGIN | QUIT ]");
            else
				Console.WriteLine("[ DECK | BATTLE | TRADE | BUY | SCORES | LOGOUT | QUIT ]");

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

        public IUser RegisterUser() {
	        Console.Write("Username: ");
	        string username = Console.ReadLine();
	        Console.Write("Password: ");
	        string password = Console.ReadLine();

	        return new User(username, password); 
        }

        public IUser LoginUser() {
			Console.Write("Username: ");
			string username = Console.ReadLine();
			Console.Write("Password: ");
			string password = Console.ReadLine();

			IsLoggedIn = true;
			loggedInUser = new User(username, password);
			return loggedInUser; 
        }
    }
}
