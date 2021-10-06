using System;
using System.Collections.Generic;
using System.Text;
using MTCG.Cards;

namespace MTCG.CardCollections {
    class Stack : ICardCollection{
	    public int Capacity { get; }
	    public List<ICard> Cards { get; set; }

	    public Stack() {
		    Cards = new List<ICard>();
	    }
	}
}
