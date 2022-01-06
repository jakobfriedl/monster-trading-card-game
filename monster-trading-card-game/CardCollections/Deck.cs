using System;
using System.Collections.Generic;
using Castle.Core.Internal;
using monster_trading_card_game.Cards;
using System.Drawing;
using Console = Colorful.Console;

namespace monster_trading_card_game.CardCollections {
    public class Deck : ICardCollection {
	    public int Capacity { get; } = 4; 
	    public List<ICard> Cards { get; set; }

	    public Deck() {
		    Cards = new List<ICard>(Capacity); 
	    }
	    public void AddCard(ICard card) {
		    Cards.Add(card);
	    }
	    public void Print() {
		    Console.WriteLine($"{"#".PadRight(4)}{"Card Name".PadRight(18)}{"Damage".PadRight(10)}{"Experience".PadRight(13)}Level");

		    int i = 1;
		    foreach (var card in Cards) {
				Console.Write(i.ToString().PadRight(4));
				card.PrintCardNameInTable();
				System.Console.Write(card.Damage.ToString().PadRight(10));
				System.Console.Write(card.Experience.ToString().PadRight(13));
				for (int j = 0; j < card.Level; j++) {
					Console.Write("*", Color.Gold);
				}
				System.Console.WriteLine();
				i++;
			}
	    }
	    public void RemoveCard(ICard card) {
		    Cards.Remove(card);
		}

	    public int Count() {
		    return Cards.Count;
	    }

	    public bool IsEmpty() {
		    return Cards.IsNullOrEmpty();
	    }

	    public Deck AutoCreateDeck(CardStack cardStack) {
		    // Random Cards
		    // var rand = new Random();
		    //for (int i = 0; i < Deck.Capacity; i++) {
		    //	Deck.AddCard(CardStack.Cards.ElementAt(rand.Next(CardStack.Count()-1)));
		    //}

		    // Strongest Cards
		    for (int i = 0; i < Capacity; i++) {
			    var card = cardStack.GetHighestDamageCard();
			    AddCard(card);
			    cardStack.RemoveCard(card);
		    }

		    foreach (var card in Cards) {
			    cardStack.AddCard(card);
		    }

		    return this; 
	    }
	}
}
