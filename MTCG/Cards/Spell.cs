using System;
using System.Collections.Generic;
using System.Text;
using MTCG.Enums;

namespace MTCG.Cards {
    class Spell : ISpell{
	    public string Name { get; set; }
	    public readonly int Damage;
	    public ElementType ElementType { get; set; }

	    public Spell(string name, int damage, ElementType type) {
		    Name = name;
		    Damage = damage;
		    ElementType = type;
	    }
	}
}
