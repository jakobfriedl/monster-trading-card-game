using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monster_trading_card_game.Enums {
    class Weakness {
	    public Dictionary<ElementType, List<ElementType>> ElementWeakness = new Dictionary<ElementType, List<ElementType>>() {
		    { ElementType.Water, new List<ElementType> { ElementType.Normal , ElementType.Electric} },
		    { ElementType.Normal, new List<ElementType> { ElementType.Fire , ElementType.Ice}  },
		    { ElementType.Fire, new List<ElementType> { ElementType.Water , ElementType.Ground} },
		    { ElementType.Electric, new List<ElementType> { ElementType.Normal, ElementType.Ground } },
		    { ElementType.Ice, new List<ElementType> { ElementType.Fire, ElementType.Electric } },
		    { ElementType.Ground, new List<ElementType> { ElementType.Water, ElementType.Ice } },
	    };
    }
}
