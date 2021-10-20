using System;
using monster_trading_card_game.CardCollections;
using monster_trading_card_game.Cards;
using monster_trading_card_game.Enums;
using monster_trading_card_game.Users;

namespace monster_trading_card_game {
    class Program {
        static void Main(string[] args) {
            User user1 = new User("test", "1234");
            User user2 = new User("test2", "2345");

            user1.GenerateCardStack();
            user2.GenerateCardStack();

            user1.Challenge(user2);
        }
    }
}
