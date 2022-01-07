using System.Collections.Generic;
using monster_trading_card_game.Cards;

namespace monster_trading_card_game.CardCollections {
    public interface ICardCollection {
        int Capacity { get; }
	    List<ICard> Cards { get; set; }
	    void AddCard(ICard card);
	    void RemoveCard(ICard card); 
	    int Count();
	    bool IsEmpty();
    }
}
