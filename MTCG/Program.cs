using System;
using MTCG.CardCollections;
using MTCG.Cards;
using MTCG.Enums;
using MTCG.Users;

namespace MTCG {
    class Program {
        static void Main(string[] args) {
	        User test = new User("test", "1234");

	        Deck deck = new Deck();
            deck.AddCard(new Spell("asd", 20, ElementType.Fire));
            deck.AddCard(new Spell("asd", 20, ElementType.Fire));

            deck.AddCard(new Spell("asd", 20, ElementType.Fire));

            deck.AddCard(new Spell("asd", 20, ElementType.Fire));
            deck.AddCard(new Spell("asd", 20, ElementType.Fire));
            deck.AddCard(new Spell("asd", 20, ElementType.Fire));

            deck.AddCard(new Spell("asd", 20, ElementType.Fire));

            deck.AddCard(new Spell("asd", 20, ElementType.Fire));

            deck.AddCard(new Spell("asd", 20, ElementType.Fire));

            deck.PrintDeck();
        }
    }
}
