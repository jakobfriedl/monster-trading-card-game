using System;
using System.Collections.Generic;
using System.Text;
using MTCG.Cards;

namespace MTCG.CardCollections {
    interface ICardCollection {
        int Capacity { get; }
	    List<ICard> Cards { get; set; }
    }
}
