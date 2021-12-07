using System;
using System.Collections.Generic;
using System.Text;
using monster_trading_card_game.Enums;

namespace monster_trading_card_game.Cards {
    public class Monster : IMonster {
		public int Id { get; set; }
	    public string Name { get; set; }
	    public int Damage { get; }
	    public ElementType ElementType { get; set; }
	    public MonsterType MonsterType { get; set; }

		public Monster(int id, string name, int damage, ElementType eType, MonsterType mType) {
			Id = id;
		    Name = name;
		    Damage = damage;
		    ElementType = eType;
		    MonsterType = mType;
			
		}
	}
}
