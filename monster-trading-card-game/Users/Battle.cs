using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using monster_trading_card_game.Cards;
using monster_trading_card_game.Enums;

namespace monster_trading_card_game.Users {
	public class Battle {
		private const int MaxRounds = 100;
		private const int WeaknessFactor = 2; 

		private readonly IUser _player1;
		private readonly IUser _player2;

		public Battle(IUser user1, IUser user2) {
			this._player1 = user1;
			this._player2 = user2;
		}

		public IUser StartBattle() {
			Console.WriteLine("Player 1 Stack:");
			_player1.CardStack.Print();
			Console.WriteLine("Player 2 Stack:");
			_player2.CardStack.Print();
			Console.WriteLine("Player 1 Deck:");
			_player1.Deck.Print();
			Console.WriteLine("Player 2 Deck:");
			_player2.Deck.Print();
			Console.WriteLine();

			for (int i = 0; i < MaxRounds; i++) {
				ICard roundWinner;   
				ICard card1 = _player1.ChooseRandomCard();
				ICard card2 = _player2.ChooseRandomCard();

				Console.WriteLine($"--ROUND {i}: {card1.Name}|{card1.Damage} VS: {card2.Name}|{card2.Damage} --");

				if (card1.GetType().Name == "Monster" && card2.GetType().Name == "Monster") {
					roundWinner = MonsterBattle(card1, card2); 
				}else if (card1.GetType().Name == "Spell" && card2.GetType().Name == "Spell") {
					roundWinner = SpellBattle(card1, card2);
				} else {
					roundWinner = MixedBattle(card1, card2); 
				}

				if (roundWinner == card1) {
					_player2.Deck.RemoveCard(card2);
					_player1.AddCardToDeck(card2);
					Console.WriteLine($"Player 1 won round {i}! He now has {_player1.Deck.Count()} cards while Player 2 has {_player2.Deck.Count()}.");
				} else if (roundWinner == card2) {
					_player1.Deck.RemoveCard(card1);
					_player2.AddCardToDeck(card1);
					Console.WriteLine($"Player 2 won round {i}! He now has {_player2.Deck.Count()} cards while Player 1 has {_player1.Deck.Count()}.");
				} else {
					Console.WriteLine($"Draw in round {i}!");
				}

				Console.WriteLine();

				if (_player1.Deck.IsEmpty()) {
					Console.WriteLine("Player 2 wins the game!");
					_player2.WinGame();
					_player1.LoseGame();
					return _player2; 
				}

				if (_player2.Deck.IsEmpty()) {
					Console.WriteLine("Player 1 wins the game!");
					_player1.WinGame();
					_player2.LoseGame();
					return _player1;
				}
			}
			Console.WriteLine($"{MaxRounds} rounds are over. No one wins!");
			return null; 
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

			// Check higher Damage
			if (card1.Damage == card2.Damage) return null;
			return card1.Damage > card2.Damage ? card1 : card2; 
		}

		public ICard SpellBattle(ICard card1, ICard card2) {
			int damage1 = card1.Damage;
			int damage2 = card2.Damage; 

			if (card1.ElementType == card2.ElementWeakness) {
				damage1 *= WeaknessFactor;
				damage2 /= WeaknessFactor;
			}

			if (card2.ElementType == card1.ElementWeakness) {
				damage1 /= WeaknessFactor;
				damage2 *= WeaknessFactor; 
			}

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

			if (card1.ElementType == card2.ElementWeakness) {
				damage1 *= WeaknessFactor;
				damage2 /= WeaknessFactor;
			}

			if (card2.ElementType == card1.ElementWeakness) {
				damage1 /= WeaknessFactor;
				damage2 *= WeaknessFactor;
			}

			if (damage1 == damage2) return null;
			return damage1 > damage2 ? card1 : card2;
		}
	}
}
