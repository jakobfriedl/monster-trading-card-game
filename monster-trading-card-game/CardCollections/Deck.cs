using System;
using System.Collections.Generic;
using System.Text;
using Castle.Core.Internal;
using monster_trading_card_game.Cards;

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
		    foreach (ICard card in Cards) {
				Console.WriteLine($"	{card.Name} - {card.Damage}");
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
