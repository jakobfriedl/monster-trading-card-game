using System;
using System.Collections.Generic;
using System.Drawing;
using Castle.Core.Internal;
using monster_trading_card_game.Cards;
using monster_trading_card_game.Enums;
using Console = Colorful.Console; 

namespace monster_trading_card_game.CardCollections {
    public class Package : ICardCollection {
	    private const int MinDamage = 50;
	    private const int MaxDamage = 100;
	    public int Cost { get; } = 5; 
		public int Capacity { get; } = 5;
	    public List<ICard> Cards { get; set; }

	    public Package(PackageType type) {
		    Cards = new List<ICard>(Capacity);
			GeneratePackage(type);
	    }

	    public void AddCard(ICard card) {
		    if (Cards.Count < Capacity) {
			    Cards.Add(card);
		    }
	    }

	    public void Print() {
			Console.WriteLine($"{"#".PadRight(4)}{"Card Name".PadRight(18)}{"Damage".PadRight(10)}{"Experience".PadRight(13)}Level");

			int i = 1;
			foreach (var card in Cards) {
				Console.Write(i.ToString().PadRight(4));
				card.PrintCardName();
				System.Console.Write(card.Damage.ToString().PadRight(10));
				System.Console.Write(card.Experience.ToString().PadRight(13));
				for (int j = 0; j < card.Level; j++) {
					Console.Write("*", Color.Gold);
				}
				System.Console.WriteLine();
				i++;
			}
		}

	    public void RemoveCard(ICard card) {
		    Cards.Remove(card);
		}

	    public int Count() {
		    return Cards.Count;
	    }

	    public bool IsEmpty() {
		    return Cards.IsNullOrEmpty();
	    }

	    public void GeneratePackage(PackageType type) {
		    var rand = new Random();

		    if (type == PackageType.Monster) {
				for (int i = 0; i < Capacity; i++) {
					int damage = rand.Next(MinDamage, MaxDamage);
					ElementType element = (ElementType)rand.Next(Enum.GetNames(typeof(ElementType)).Length);
					MonsterType monster = (MonsterType)rand.Next(Enum.GetNames(typeof(MonsterType)).Length) + 1; 
					string name = $"{element} {monster}";

					Cards.Add(new Monster(0, name, damage, element, monster));
				}

				return; 
		    }

		    if (type == PackageType.Spell) {
			    for (int i = 0; i < Capacity; i++) {
				    int damage = rand.Next(MinDamage, MaxDamage);
				    ElementType element = (ElementType)rand.Next(Enum.GetNames(typeof(ElementType)).Length);
				    string name = $"{element} Spell";

				    Cards.Add(new Spell(0, name, damage, element));
			    }

			    return; 
		    }

		    for (int i = 0; i < Capacity; i++) {
			    int damage = rand.Next(MinDamage, MaxDamage);
			    ElementType element = (ElementType)type; 
				MonsterType monster = (MonsterType)rand.Next(Enum.GetNames(typeof(MonsterType)).Length+1);

				if (monster == 0) {
					string name = $"{element} Spell";
					Cards.Add(new Spell(0, name, damage, element));
				} else {
					string name = $"{element} {monster}";
					Cards.Add(new Monster(0, name, damage, element, monster));
				}
		    }
	    }
    }
}
