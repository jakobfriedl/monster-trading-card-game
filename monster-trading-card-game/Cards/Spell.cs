using System;
using System.Drawing;
using monster_trading_card_game.Enums;
using Colorful;
using Console = Colorful.Console; 

namespace monster_trading_card_game.Cards {
    public class Spell : ISpell{
		public int Id { get; set; }
	    public string Name { get; set; }
	    public int Damage { get; }
	    public ElementType ElementType { get; set; }
		public MonsterType MonsterType { get; set; }

		public Spell(int id, string name, int damage, ElementType type) {
			Id = id;
		    Name = name;
		    Damage = damage;
		    ElementType = type;
		}
		public void PrintCardName() {
			Console.Write(ElementType, ElementType == ElementType.Fire ? Color.Firebrick : ElementType == ElementType.Water ? Color.DodgerBlue : Color.Gray);
			Console.Write(" Spell", Color.DarkViolet);
		}
	}
}
