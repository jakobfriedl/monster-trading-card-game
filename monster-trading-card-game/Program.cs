using System;
using monster_trading_card_game.Enums;
using monster_trading_card_game.Database;
using Colorful;
using System.Drawing;
using System.Threading;
using Console = Colorful.Console; 

namespace monster_trading_card_game {
    class Program {
        static void Main(string[] args) {
	        CLI cli = new CLI();

	        Command cmd; 
            while ((cmd = cli.GetUserCommand()) != Command.Quit) {
				switch (cmd) {
                    case Command.Register:
	                    if (cli.RegisterUser()) {
		                    Console.WriteLine("\nRegistration successful", Color.ForestGreen);
							Thread.Sleep(1000);
	                    } else {
		                    Console.WriteLine("\nRegistration failed", Color.Red);
							Thread.Sleep(1000);
	                    }
	                    break;
                    case Command.Login:
	                    if (cli.LoginUser()) {
		                    Console.WriteLine("\nLogin successful", Color.ForestGreen);
		                    Thread.Sleep(1000);
	                    } else {
		                    Console.WriteLine("\nLogin failed", Color.Red);
		                    Thread.Sleep(1000);
	                    }
						break;
                    case Command.Deck:
                        cli.LoggedInUser.BuildDeck();
	                    break;
                    case Command.Trade:
	                    cli.Trade(); 
	                    break; 
                    case Command.Battle:
	                    cli.Battle();
	                    break;
                    case Command.Buy:
	                    cli.GetPackage(); 
	                    break;
                    case Command.Scores:
	                    cli.Scores(); 
	                    break;
					case Command.Profile:
						cli.Profile();
						break;
                    case Command.Logout:
	                    cli.LogoutUser();
	                    break; 
                    case Command.Quit:
	                    break; 
                    case Command.Invalid:
	                    break;
	            }
            }
        }
    }
}
