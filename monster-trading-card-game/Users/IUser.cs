using System;
using System.Collections.Generic;
using System.Text;
using monster_trading_card_game.CardCollections;
using monster_trading_card_game.Cards;

namespace monster_trading_card_game.Users {
	public interface IUser {
		int Id { get; set; }
		string Username { get; set; }
		string Password { get; set; }
		int Coins { get; set; }
		int Elo { get; set; }
		int Wins { get; set; }
		int Losses { get; set; }
		CardStack CardStack { get; set; }
		Deck Deck { get; set; }

		//Functions
		void Challenge(IUser opponent);
		void AutoCreateDeck();
		void BuildDeck();
		ICard ChooseRandomCard();
		void AddCardToStack(ICard card);
		void AddCardToDeck(ICard card);
		void WinGame();
		void LoseGame();
		void Print();
		void BuyPackage(Package package);
		void OfferCard();
		void ManageOwnOffers();
		void FindOtherOffers();
		void ShowTransactions();
		void SendBattleRequest();
		bool HandleBattleRequests();
	}
}
