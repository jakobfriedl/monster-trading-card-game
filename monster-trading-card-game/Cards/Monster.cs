using System;
using System.Drawing;
using monster_trading_card_game.Enums;
using Colorful;
using Console = Colorful.Console;

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

		public void PrintCardName() {
			Console.Write(ElementType, ElementType == ElementType.Fire ? Color.Firebrick : ElementType == ElementType.Water ? Color.DodgerBlue : Color.Gray);
			Console.Write(" " + MonsterType, Color.Green);
			Console.Write(" ".PadRight(15 - (ElementType.ToString().Length + MonsterType.ToString().Length + 1)));
		}
	}
}
