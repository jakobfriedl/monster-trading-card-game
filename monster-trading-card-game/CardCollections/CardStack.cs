using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Castle.Core.Internal;
using monster_trading_card_game.Cards;
using monster_trading_card_game.Enums;
using Console = Colorful.Console; 

namespace monster_trading_card_game.CardCollections {
    public class CardStack : ICardCollection {
	    private const int MinDamage = 50;
	    private const int MaxDamage = 101;
	    private const int NumSpells = 2;
	    private const int NumMonsters = 2;
		public int Capacity { get; } = 100; 
	    public List<ICard> Cards { get; set; }

	    public CardStack() {
		    Cards = new List<ICard>();
	    }

	    public void AddCard(ICard card) {
		    Cards.Add(card);
	    }

	    public void Print() {
			Console.WriteLine($"{"#".PadRight(4)}{"Card Name".PadRight(18)}{"Damage".PadRight(10)}{"Experience".PadRight(13)}Level");

			int i = 1;
			foreach (var card in Cards) {
				Console.Write(i.ToString().PadRight(4));
				card.PrintCardNameInTable();
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

	    public ICard GetHighestDamageCard() {
		    var max = Cards.First();

		    foreach (var card in Cards) {
			    if (card.Damage > max.Damage)
				    max = card; 
		    }
		    return max; 
	    }

	    public CardStack GenerateRandomSpells(int count) {
		    CardStack spellStack = new CardStack();
		    var rand = new Random();
		    for (int i = 0; i < count; i++) {
			    int damage = rand.Next(MinDamage, MaxDamage);
			    ElementType element = (ElementType)rand.Next(Enum.GetNames(typeof(ElementType)).Length);
			    string name = $"{element} Spell";

			    spellStack.AddCard(new Spell(0, name, damage, element));
		    }
		    return spellStack;
	    }

	    public CardStack GenerateRandomMonsters(int count) {
		    CardStack monsterStack = new CardStack();
		    var rand = new Random();
		    for (int i = 0; i < count; i++) {
			    int damage = rand.Next(MinDamage, MaxDamage);
			    ElementType element = (ElementType)rand.Next(Enum.GetNames(typeof(ElementType)).Length);
			    MonsterType monster = (MonsterType)rand.Next(Enum.GetNames(typeof(MonsterType)).Length) + 1;
			    string name = $"{element} {monster}";

			    monsterStack.AddCard(new Monster(0, name, damage, element, monster));
		    }
		    return monsterStack;
	    }

	    public CardStack GenerateCardStack() {
		    //var rand = new Random();
		    //int numberOfSpells = rand.Next(MinSpells, MaxSpells);
		    //int numberOfMonsters = InitialStackCapacity - numberOfSpells;

		    var cards = new CardStack(); 

		    foreach (var spell in GenerateRandomSpells(NumSpells).Cards) {
			    cards.AddCard(spell);
		    }

		    foreach (var monster in GenerateRandomMonsters(NumMonsters).Cards) {
			    cards.AddCard(monster);
		    }

		    return cards;
	    }
	}
}
