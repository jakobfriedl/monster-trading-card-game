﻿using System;
using System.Collections.Generic;
using System.Text;
using monster_trading_card_game.Enums;

namespace monster_trading_card_game.Cards {
    class Monster : IMonster {
	    public string Name { get; set; }
	    public readonly int Damage;
	    public ElementType ElementType { get; set; }
	    public MonsterType MonsterType { get; set; }

	    public Monster(string name, int damage, ElementType eType, MonsterType mType) {
		    Name = name;
		    Damage = damage;
		    ElementType = eType;
		    MonsterType = mType; 
	    }
	}
}