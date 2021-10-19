using System;
using System.Collections.Generic;
using System.Text;
using monster_trading_card_game.Cards;

namespace monster_trading_card_game.CardCollections {
    interface ICardCollection {
        int Capacity { get; }
	    List<ICard> Cards { get; set; }

	    void AddCard(ICard card);
	    void RemoveCard(ICard card); 
	    void Print(); 
    }
}
