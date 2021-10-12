using System;
using System.Collections.Generic;
using System.Text;
using MTCG.Cards;

namespace MTCG.CardCollections {
    public class Stack : ICardCollection{
	    public int Capacity { get; }
	    public List<ICard> Cards { get; set; }

	    public Stack() {
		    Cards = new List<ICard>();
	    }

	    public void AddCard(ICard card) {
		    Cards.Add(card);
	    }

	    public void Print() {
		    foreach (ICard card in Cards) {
			    Console.WriteLine(card.Name);
		    }
	    }
	}
}
