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
				card.PrintCardName();
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

	}
}
