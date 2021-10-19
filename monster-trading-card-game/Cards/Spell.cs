using System;
using System.Collections.Generic;
using System.Text;
using monster_trading_card_game.Enums;

namespace monster_trading_card_game.Cards {
    class Spell : ISpell{
	    public string Name { get; set; }
	    public int Damage { get; }
	    public ElementType ElementType { get; set; }

	    public Spell(string name, int damage, ElementType type) {
		    Name = name;
		    Damage = damage;
		    ElementType = type;
	    }
	}
}
