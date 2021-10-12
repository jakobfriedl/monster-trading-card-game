using System;
using System.Collections.Generic;
using System.Text;
using monster_trading_card_game.Enums;

namespace monster_trading_card_game.Cards {
    public interface ICard {
	    public string Name { get; set; }
	    public ElementType ElementType { get; set; }
    }
}
