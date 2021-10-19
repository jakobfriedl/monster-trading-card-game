using System;
using System.Collections.Generic;
using System.Text;
using monster_trading_card_game.Enums;

namespace monster_trading_card_game.Cards {
    public interface ICard {
        int Damage { get; }
	    string Name { get; set; }
	    ElementType ElementType { get; set; }

        
    }
}
