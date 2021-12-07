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
	        UI ui = new UI();

	        DBConnection db = new DBConnection(); 
            db.Connect();

            Command cmd; 
            while ((cmd = ui.GetUserCommand()) != Command.Quit) {
	            switch (cmd) {
                    case Command.Register:
	                    if (ui.RegisterUser()) {
		                    Console.WriteLine("Registration successful");
	                    } else {
		                    Console.WriteLine("Registration failed");
	                    }
	                    break;
                    case Command.Login:
	                    if (ui.LoginUser()) {
		                    Console.WriteLine("Login successful");
	                    } else {
		                    Console.WriteLine("Login failed");
	                    }
	                    break;
                    case Command.Deck:
                        ui.LoggedInUser.BuildDeck();
	                    break;
                    case Command.Trade:
	                    break; 
                    case Command.Battle:
	                    IUser bot = new User("Bot", "da39a3ee5e6b4b0d3255bfef95601890afd80709");
                        ui.LoggedInUser.Print();
                        ui.LoggedInUser.Challenge(bot);
	                    break;
                    case Command.Buy:
	                    break;
                    case Command.Scores:
	                    break;
                    case Command.Logout:
	                    ui.LogoutUser();
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
