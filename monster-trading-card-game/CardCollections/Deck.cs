using System;
using System.Collections.Generic;
using System.Text;
using monster_trading_card_game.Cards;

namespace monster_trading_card_game.CardCollections {
    public class Deck : ICardCollection {
	    public int Capacity { get; } = 4; 
	    public List<ICard> Cards { get; set; }

	    public Deck() {
		    Cards = new List<ICard>(Capacity); 
	    }

	    public void AddCard(ICard card) {
			if(Cards.Count < Capacity)
				Cards.Add(card);
	    }

	    public void Print() {
		    foreach (ICard card in Cards) {
				Console.WriteLine(card.Name);
		    }
	    }

	    public void RemoveCard(ICard card) {

	    }

	}
}
