using System;
using System.IO;
using System.Text.Json.Serialization;
using Castle.DynamicProxy.Generators;
using Microsoft.VisualBasic.CompilerServices;
using monster_trading_card_game.CardCollections;
using monster_trading_card_game.Cards;
using monster_trading_card_game.Enums;
using monster_trading_card_game.Users;
using Newtonsoft.Json;

namespace monster_trading_card_game {
    class Program {
        static void Main(string[] args) {
	        UI ui = new UI(); 

            //User user1 = new User("test", "1234");
            //User user2 = new User("test2", "2345");
            // user1.Challenge(user2);

            Command cmd; 
            while ((cmd = ui.GetUserCommand()) != Command.Quit) {
	            switch (cmd) {
                    case Command.Register:
	                    IUser registerUser= ui.RegisterUser();
                        registerUser.Print();
	                    break;
                    case Command.Login:
	                    IUser loginUser = ui.LoginUser();
	                    loginUser.Print(); 
	                    break;
                    case Command.Deck:
                        ui.loggedInUser.BuildDeck();
	                    break;
                    case Command.Trade:
	                    break; 
                    case Command.Battle:
	                    IUser bot = new User("Bot", "da39a3ee5e6b4b0d3255bfef95601890afd80709");
                        ui.loggedInUser.Print();
                        ui.loggedInUser.Challenge(bot);
	                    break;
                    case Command.Buy:
	                    break;
                    case Command.Scores:
	                    break; 
                    case Command.Quit:
	                    break; 
                    case Command.Invalid:
	                    break;
	            }
            }

            //Console.WriteLine(user1.Deck.Count());
            //Console.WriteLine(user2.Deck.Count());

            //string json = JsonConvert.SerializeObject(user1);
            //string path = $@"C:\.Jakob\FHTW\3_Semester\Sofware_Engineering\monster-trading-card-game\SerializedUsers\{user1.id}.json";

            //if (!File.Exists(path)) {
            //    File.WriteAllText(path, json);
            //}
        }
    }
}
