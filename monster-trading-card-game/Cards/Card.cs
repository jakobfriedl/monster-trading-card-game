
using System;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using monster_trading_card_game.Database;
using monster_trading_card_game.Enums;
using Npgsql.Replication.PgOutput.Messages;

namespace monster_trading_card_game.Cards {
    public abstract class Card : ICard {
	    private const int LevelUpThreshold = 1000;  
	    private const int MaxLevel = 5;
	    private const int DamageIncrease = 3; 
	    protected const double CriticalChanceMultiplier = 0.1;

	    public int Id { get; set; }
	    public string Name { get; set; }
	    public int Damage { get; set;  }
	    public ElementType ElementType { get; set; }
	    public MonsterType MonsterType { get; set; }
	    public int Level { get; set; }
	    public int Experience { get; set; }
	    public double CriticalChance { get; set; }

	    public abstract void PrintCardName();
	    public abstract void PrintWithDamage();
	    public abstract void PrintWithDamage(int damage);
	    public void LevelUp(int exp) {
		    Experience += exp;
		    if (Experience >= LevelUpThreshold && Level < MaxLevel) {
			    Experience = 0;
			    Level++;
			    Damage += DamageIncrease;
			    CriticalChance = CriticalChanceMultiplier * Level; 
			    Console.WriteLine("---- LEVEL UP ----");
				PrintWithDamage();
				Console.WriteLine($" gained {exp} experience points and leveled up to level {Level}!");
				Console.WriteLine("------------------");
		    }

		    var dbCard = new DBCard();
		    dbCard.UpdateCardLevel(this); 
	    }
	}
}
