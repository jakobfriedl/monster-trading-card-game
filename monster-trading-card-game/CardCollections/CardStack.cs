using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Castle.Core.Internal;
using monster_trading_card_game.Cards;

namespace monster_trading_card_game.CardCollections {
    public class CardStack : ICardCollection {
	    public int Capacity { get; } = 100; 
	    public List<ICard> Cards { get; set; }

	    public CardStack() {
		    Cards = new List<ICard>();
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

	    public ICard GetHighestDamageCard() {
		    var max = Cards.First(); 

		    foreach (var card in Cards) {
			    if (card.Damage > max.Damage)
				    max = card; 
		    }

		    return max; 
	    }
	}
}
