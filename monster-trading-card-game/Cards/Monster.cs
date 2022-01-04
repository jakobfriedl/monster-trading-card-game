using System;
using System.Drawing;
using monster_trading_card_game.Enums;
using Colorful;
using Console = Colorful.Console;

namespace monster_trading_card_game.Cards {
    public class Monster : IMonster {
	    private readonly Color _color;
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

		    switch (ElementType) {
			    case ElementType.Water:
				    _color = Color.DodgerBlue;
				    break;
			    case ElementType.Fire:
				    _color = Color.Firebrick;
				    break;
			    case ElementType.Normal:
				    _color = Color.Gray;
				    break;
			    case ElementType.Electric:
				    _color = Color.Yellow;
				    break;
				case ElementType.Ice:
					_color = Color.LightBlue;
					break;
			    case ElementType.Ground:
				    _color = Color.SaddleBrown;
				    break;
		    }
		}

		public void PrintCardName() {
			Console.Write(ElementType, _color);
			Console.Write(" " + MonsterType, Color.Green);
			Console.Write(" ".PadRight(15 - (ElementType.ToString().Length + MonsterType.ToString().Length + 1)));
		}

		public void PrintWithDamage() {
			Console.Write(ElementType, _color);
			Console.Write(" " + MonsterType, Color.Green);
			Console.Write($" ({Damage})");
		}
	}
}
