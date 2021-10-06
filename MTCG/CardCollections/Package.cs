using System;
using System.Collections.Generic;
using System.Text;
using MTCG.Cards;

namespace MTCG.CardCollections {
    class Package : ICardCollection {
	    public int Capacity { get; } = 5;
	    public List<ICard> Cards { get; set; }

	    public Package() {
		    Cards = new List<ICard>(Capacity);
	    }
	}
}
