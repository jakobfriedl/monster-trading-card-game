using System;
using System.Collections.Generic;
using System.Text;
using monster_trading_card_game.Enums;

namespace monster_trading_card_game.Cards {
    public class Monster : IMonster {
	    public string Name { get; set; }
	    public int Damage { get; }
	    public ElementType ElementType { get; set; }
	    public MonsterType MonsterType { get; set; }
		public ElementType ElementWeakness { get; }

		public Monster(string name, int damage, ElementType eType, MonsterType mType) {
		    Name = name;
		    Damage = damage;
		    ElementType = eType;
		    MonsterType = mType;
			switch (eType) {
				case ElementType.Fire:
					ElementWeakness = ElementType.Water; break;
				case ElementType.Water:
					ElementWeakness = ElementType.Normal; break;
				case ElementType.Normal:
					ElementWeakness = ElementType.Fire; break;
			}
		}
	}
}
