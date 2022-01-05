using System;
using System.Drawing;
using monster_trading_card_game.Cards;
using monster_trading_card_game.Database;
using monster_trading_card_game.Enums;
using monster_trading_card_game.Users;
using Console = Colorful.Console; 

namespace monster_trading_card_game.Battle {
	public class Battle {
		private const int MaxRounds = 100;
		private const int WeaknessFactor = 2;
		private const int CriticalHit = 2;
		private const int ExpGainBase = 100;
		private const int ExpLevelMultiplier = 20;
		private const int BeatingStrongerOpponentBonus = 20;

		private readonly Weakness _weakness = new();

		private readonly IUser _player1;
		private readonly IUser _player2;

		public Battle(IUser user1, IUser user2) {
			_player1 = user1;
			_player2 = user2;
		}

		public IUser StartBattle() {
			for (int i = 0; i < MaxRounds; i++) {
				ICard card1 = _player1.ChooseRandomCard();
				ICard card2 = _player2.ChooseRandomCard();

				System.Console.WriteLine($"==================[ROUND {i}]=====================");
				Round(i, card1, card2);
				System.Console.WriteLine("=====================================================");

				System.Console.WriteLine(); 
				// Player 1 Wins
				if (_player2.Deck.IsEmpty()) {
					Console.WriteLine($"{_player1.Username} wins the game!", Color.IndianRed);
					_player1.WinGame();
					_player2.LoseGame();
					ResetDecks();
					return _player1;
				}

				// Player 2 Wins
				if (_player1.Deck.IsEmpty()) {
					Console.WriteLine($"{_player2.Username} wins the game!", Color.CornflowerBlue);
					_player2.WinGame();
					_player1.LoseGame();
					ResetDecks();
					return _player2; 
				}
			}
			Console.WriteLine($"{MaxRounds} rounds are over. No one wins!", Color.DarkGoldenrod);
			ResetDecks();
			return null; 
		}

		public void Round(int round, ICard card1, ICard card2) {
			ICard roundWinner;

			System.Console.WriteLine("Before damage calculation:");
			card1.PrintWithDamage();
			System.Console.Write("    VS    ");
			card2.PrintWithDamage();
			System.Console.WriteLine();

			if (card1 is Monster && card2 is Monster) {
				roundWinner = MonsterBattle(card1, card2);
			} else if (card1 is Spell && card2 is Spell) {
				roundWinner = SpellBattle(card1, card2);
			} else {
				roundWinner = MixedBattle(card1, card2);
			}

			if (roundWinner == card1) {
				// Move Card to Winner
				_player2.RemoveCardFromDeck(card2);
				_player1.AddCardToDeck(card2);
				
				// Level Up Card
				card1.LevelUp(ExpGainBase - ExpLevelMultiplier*card1.Level + (card2.Level > card1.Level ? BeatingStrongerOpponentBonus : 0));

				Console.WriteLine($"{_player1.Username} won round {round}! They now have {_player1.Deck.Count()} cards while {_player2.Username} has {_player2.Deck.Count()}.", Color.IndianRed);
			} else if (roundWinner == card2) {
				// Move Card to Winner
				_player1.RemoveCardFromDeck(card1);
				_player2.AddCardToDeck(card1);

				// Level Up Card
				card2.LevelUp(ExpGainBase - ExpLevelMultiplier*card2.Level+ (card1.Level > card2.Level ? BeatingStrongerOpponentBonus : 0));

				Console.WriteLine($"{_player2.Username} won round {round}! They now have {_player2.Deck.Count()} cards while { _player1.Username} has {_player1.Deck.Count()}.", Color.CornflowerBlue);
			} else {
				Console.WriteLine($"Draw in round {round}!", Color.DarkGoldenrod);
			}
		}

		public ICard MonsterBattle(ICard card1, ICard card2) {
			// Pure Monster Fight, not affected by element type
			// Goblin is too afraid of Dragon to attack
			if ((card1.MonsterType == MonsterType.Goblin && card2.MonsterType == MonsterType.Dragon))
				return card2;
			if ((card2.MonsterType == MonsterType.Goblin && card1.MonsterType == MonsterType.Dragon))
				return card1;

			// Wizard can control Orc
			if ((card1.MonsterType == MonsterType.Wizard && card2.MonsterType == MonsterType.Orc))
				return card1;
			if ((card2.MonsterType == MonsterType.Wizard && card1.MonsterType == MonsterType.Orc))
				return card2;

			// FireElves can evade Dragon attacks
			if ((card1.MonsterType == MonsterType.Elf && card1.ElementType == ElementType.Fire && card2.MonsterType == MonsterType.Dragon))
				return card1;
			if ((card2.MonsterType == MonsterType.Elf && card2.ElementType == ElementType.Fire && card1.MonsterType == MonsterType.Dragon))
				return card2;

			// TODO: Check for Critical Hit
			var damage1 = card1.Damage;
			var damage2 = card2.Damage;

			var random = new Random();
			if (random.NextDouble() <= card1.CriticalChance) {
				Console.Write("CRITICAL HIT: ", Color.Red);
				card1.PrintWithDamage();
				System.Console.WriteLine(" lands a critical hit!");
				damage1 *= CriticalHit;
			}
			if (random.NextDouble() <= card2.CriticalChance) {
				Console.Write("CRITICAL HIT: ", Color.Red);
				card2.PrintWithDamage();
				System.Console.WriteLine(" lands a critical hit!");
				damage2 *= CriticalHit;
			}

			System.Console.WriteLine("After damage calculation:");
			card1.PrintWithDamage(damage1);
			System.Console.Write("    VS    ");
			card2.PrintWithDamage(damage2);
			System.Console.WriteLine();

			// Check higher Damage
			if (damage1 == damage2) return null;
			return damage1 > damage2 ? card1 : card2; 
		}

		public ICard SpellBattle(ICard card1, ICard card2) {
			var damage1 = card1.Damage;
			var damage2 = card2.Damage; 

			if (_weakness.ElementWeakness[card2.ElementType].Contains(card1.ElementType)) {
				damage1 *= WeaknessFactor;
				damage2 /= WeaknessFactor;
			}

			if (_weakness.ElementWeakness[card1.ElementType].Contains(card2.ElementType)) {
				damage1 /= WeaknessFactor;
				damage2 *= WeaknessFactor; 
			}

			var random = new Random();
			if (random.NextDouble() <= card1.CriticalChance) {
				Console.Write("CRITICAL HIT: ", Color.Red);
				card1.PrintWithDamage();
				System.Console.WriteLine(" lands a critical hit!");
				damage1 *= CriticalHit;
			}
			if (random.NextDouble() <= card2.CriticalChance) {
				Console.Write("CRITICAL HIT: ", Color.Red);
				card2.PrintWithDamage();
				System.Console.WriteLine(" lands a critical hit!");
				damage2 *= CriticalHit;
			}

			System.Console.WriteLine("After damage calculation:");
			card1.PrintWithDamage(damage1);
			System.Console.Write("    VS    ");
			card2.PrintWithDamage(damage2);
			System.Console.WriteLine();

			if (damage1 == damage2) return null;
			return damage1 > damage2 ? card1 : card2; 
		}

		public ICard MixedBattle(ICard card1, ICard card2) {
			int damage1 = card1.Damage;
			int damage2 = card2.Damage;

			// Kraken is immune to spells
			if (card1.MonsterType == MonsterType.Kraken) return card1;
			if (card2.MonsterType == MonsterType.Kraken) return card2;

			// Knights instantly loose to Water Spells
			if (card1.MonsterType == MonsterType.Knight && card2.GetType().Name == "Spell" && card2.ElementType == ElementType.Water) return card2; 
			if (card2.MonsterType == MonsterType.Knight && card1.GetType().Name == "Spell" && card1.ElementType == ElementType.Water) return card1;

			if (_weakness.ElementWeakness[card2.ElementType].Contains(card1.ElementType)) {
				damage1 *= WeaknessFactor;
				damage2 /= WeaknessFactor;
			}

			if (_weakness.ElementWeakness[card1.ElementType].Contains(card2.ElementType)) {
				damage1 /= WeaknessFactor;
				damage2 *= WeaknessFactor;
			}

			var random = new Random();
			if (random.NextDouble() <= card1.CriticalChance) {
				Console.Write("CRITICAL HIT: ", Color.Red);
				card1.PrintWithDamage();
				System.Console.WriteLine(" lands a critical hit!");
				damage1 *= CriticalHit;
			}
			if (random.NextDouble() <= card2.CriticalChance) {
				Console.Write("CRITICAL HIT: ", Color.Red);
				card2.PrintWithDamage();
				System.Console.WriteLine(" lands a critical hit!");
				damage2 *= CriticalHit;
			}

			System.Console.WriteLine("After damage calculation:");
			card1.PrintWithDamage(damage1);
			System.Console.Write("    VS    ");
			card2.PrintWithDamage(damage2);
			System.Console.WriteLine(); 

			if (damage1 == damage2) return null;
			return damage1 > damage2 ? card1 : card2;
		}

		public void ResetDecks() {
			var dbCard = new DBCard();

			_player1.Deck = dbCard.GetDeckFromUserId(_player1.Id);
			_player2.Deck = dbCard.GetDeckFromUserId(_player2.Id);
			System.Console.WriteLine("\nPress any key to continue.");
			Console.ReadKey();
		}
	}
}
