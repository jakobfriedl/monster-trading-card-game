using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG.Cards {
    class Spell : ICard, ISpell{
	    protected string Name { get; set; }
	    protected readonly int Damage;
	    protected ElementType ElementType { get; set; }

	    protected Spell(string name, int damage, ElementType type) {
		    Name = name;
		    Damage = damage;
		    ElementType = type;
	    }
	}
}
