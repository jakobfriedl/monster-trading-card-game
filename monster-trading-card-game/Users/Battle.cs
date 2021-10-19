﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using monster_trading_card_game.Cards;
using monster_trading_card_game.Enums;

namespace monster_trading_card_game.Users {
	public class Battle {
		private const int MaxRounds = 100;

		private readonly IUser _player1;
		private readonly IUser _player2;

		public Battle(IUser user1, IUser user2) {
			this._player1 = user1;
			this._player2 = user2;
		}

		public IUser StartBattle() {
			ICard roundWinner = null;  
			for (int i = 0; i < MaxRounds; i++) {
				ICard card1 = _player1.ChooseRandomCard();
				ICard card2 = _player2.ChooseRandomCard();

				if (card1.GetType().Name == "Monster" && card2.GetType().Name == "Monster") {
					roundWinner = MonsterBattle((IMonster)card1, (IMonster)card2); 
				}else if (card1.GetType().Name == "Spell" && card2.GetType().Name == "Spell") {
					roundWinner = SpellBattle(card1, card2);
				} else {
					roundWinner = MixedBattle(card1, card2); 
				}

				if (roundWinner == card1) {
					_player2.CardStack.RemoveCard(card2);
					_player1.CardStack.AddCard(card2);
					Console.WriteLine($"Player 1 won round {i}! He now has {_player1.CardStack.Count()} cards.");
				} else if (roundWinner == card2) {
					_player1.CardStack.RemoveCard(card1);
					_player2.CardStack.AddCard(card1);
					Console.WriteLine($"Player 2 won round {i}! He now has {_player2.CardStack.Count()} cards.");
				} else {
					Console.WriteLine($"Draw in round {i}!");
				}

				if (_player1.CardStack.IsEmpty()) {
					Console.WriteLine("Player 2 wins the game!");
					_player2.WinGame();
					_player1.LoseGame();
					return _player2; 
				}

				if (_player2.CardStack.IsEmpty()) {
					Console.WriteLine("Player 1 wins the game!");
					_player1.WinGame();
					_player2.LoseGame();
					return _player1;
				}
			}
			Console.WriteLine($"{MaxRounds} rounds are over. No one wins!");
			return null; 
		}

		public ICard MonsterBattle(IMonster card1, IMonster card2) {
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

			if (card1.Damage > card2.Damage) return card1;
			if (card1.Damage < card2.Damage) return card2;
			return null; 
		}

		public ICard SpellBattle(ICard card1, ICard card2) {
			return null;
		}

		public ICard MixedBattle(ICard card1, ICard card2) {
			return null; 
		}
	}
}