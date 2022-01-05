using System;
using System.Drawing;
using monster_trading_card_game.Enums;
using Colorful;
using Console = Colorful.Console;

namespace monster_trading_card_game.Cards {
    public class Monster : Card {
	    private readonly Color _color;

	    public Monster(int id, string name, int damage, ElementType eType, MonsterType mType, int level, int exp) {
		    Id = id;
		    Name = name;
		    Damage = damage;
		    ElementType = eType;
		    MonsterType = mType;
		    Level = level;
		    Experience = exp;
		    CriticalChance = CriticalChanceMultiplier * Level;

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

		public Monster(int id, string name, int damage, ElementType eType, MonsterType mType) {
			Id = id;
		    Name = name;
		    Damage = damage;
		    ElementType = eType;
		    MonsterType = mType;
		    Level = 0;
		    Experience = 0;
		    CriticalChance = CriticalChanceMultiplier * Level;

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
				default:
					_color = Color.White;
					break;
		    }
		}

		public override void PrintCardName()  {
			Console.Write(ElementType, _color);
			Console.Write(" " + MonsterType, Color.Green);
			Console.Write(" ".PadRight(18 - (ElementType.ToString().Length + MonsterType.ToString().Length + 1)));
		}

		public override void PrintWithDamage() {
			Console.Write(ElementType, _color);
			Console.Write(" " + MonsterType, Color.Green);
			Console.Write($" ({Damage}) ");

			for (int i = 0; i < Level; i++) {
				Console.Write("*", Color.Gold);
			}
		}
		public override void PrintWithDamage(int damage) {
			Console.Write(ElementType, _color);
			Console.Write(" " + MonsterType, Color.Green);
			Console.Write($" ({damage}) ");

			for (int i = 0; i < Level; i++) {
				Console.Write("*", Color.Gold);
			}
		}
	}
}
