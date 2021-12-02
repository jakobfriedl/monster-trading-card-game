using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monster_trading_card_game.Enums {
    class Weakness {
	    public Dictionary<ElementType, ElementType> ElementWeakness = new Dictionary<ElementType, ElementType>() {
		    { ElementType.Water, ElementType.Normal },
		    { ElementType.Normal, ElementType.Fire },
		    { ElementType.Fire, ElementType.Water }
	    };
    }
}
