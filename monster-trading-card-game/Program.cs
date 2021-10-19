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

            //user1.CardStack.AddCard(new Spell("Freeze", 20, ElementType.Water));
            //user1.CardStack.AddCard(new Spell("Fireball", 40, ElementType.Fire));
            //user1.CardStack.AddCard(new Spell("Beam", 30, ElementType.Normal));
            user1.AddCardToStack(new Monster("Water Goblin", 50, ElementType.Water, MonsterType.Goblin));
            user1.AddCardToStack(new Monster("Fire Dragon", 100, ElementType.Fire, MonsterType.Dragon));
            user1.AddCardToStack(new Monster("Regular Wizard", 40, ElementType.Normal, MonsterType.Wizard));
            user1.AddCardToStack(new Monster("Water Kraken", 20, ElementType.Water, MonsterType.Kraken));
            user1.AddCardToStack(new Monster("Normal Orc", 20, ElementType.Normal, MonsterType.Orc));

            user2.AddCardToStack(new Monster("Water Goblin", 50, ElementType.Water, MonsterType.Goblin));
            user2.AddCardToStack(new Monster("Regular Wizard", 100, ElementType.Normal, MonsterType.Wizard));

            user1.Challenge(user2);

        }
    }
}
