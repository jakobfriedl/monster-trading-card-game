using System;
using System.IO;
using System.Text.Json.Serialization;
using Castle.DynamicProxy.Generators;
using Microsoft.VisualBasic.CompilerServices;
using monster_trading_card_game.CardCollections;
using monster_trading_card_game.Cards;
using monster_trading_card_game.Enums;
using monster_trading_card_game.Users;
using monster_trading_card_game.Database;
using Newtonsoft.Json;

namespace monster_trading_card_game {
    class Program {
        static void Main(string[] args) {
	        CLI cli = new CLI();

	        DBConnection db = new DBConnection(); 
            db.Connect();

            Command cmd; 
            while ((cmd = cli.GetUserCommand()) != Command.Quit) {
	            switch (cmd) {
                    case Command.Register:
	                    if (cli.RegisterUser()) {
		                    Console.WriteLine("Registration successful");
	                    } else {
		                    Console.WriteLine("Registration failed");
	                    }
	                    break;
                    case Command.Login:
	                    if (cli.LoginUser()) {
		                    Console.WriteLine("Login successful");
	                    } else {
		                    Console.WriteLine("Login failed");
	                    }
	                    break;
                    case Command.Deck:
                        cli.LoggedInUser.BuildDeck();
	                    break;
                    case Command.Trade:
	                    break; 
                    case Command.Battle:
	                    cli.Battle();
	                    break;
                    case Command.Buy:
	                    cli.GetPackage(); 
	                    break;
                    case Command.Scores:
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
