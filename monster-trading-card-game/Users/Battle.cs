using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using monster_trading_card_game.Cards;

namespace monster_trading_card_game.Users {
	class Battle {
		private const int MaxRounds = 100;

		private readonly IUser _player1;
		private readonly IUser _player2;

		public Battle(IUser user1, IUser user2) {
			this._player1 = user1;
			this._player2 = user2;
		}

		public IUser StartBattle() {
			for (int i = 0; i < MaxRounds; i++) {
				ICard card1 = _player1.ChooseRandomCard();
				ICard card2 = _player2.ChooseRandomCard();

				if (card1.GetType().Name == "Monster" && card2.GetType().Name == "Monster") {
					MonsterBattle(card1, card2); 
				}else if (card1.GetType().Name == "Spell" && card2.GetType().Name == "Spell") {
					SpellBattle(card1, card2);
				} else {
					MixedBattle(card1, card2); 
				}
			}

			return null; 
		}

		private int MonsterBattle(ICard card1, ICard card2) {
			return 0; 
		}

		private int SpellBattle(ICard card1, ICard card2) {
			return 0;
		}

		private int MixedBattle(ICard card1, ICard card2) {
			return 0; 
		}
	}
}
