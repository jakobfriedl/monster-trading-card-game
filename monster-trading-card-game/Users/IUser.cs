using System;
using System.Collections.Generic;
using System.Text;
using monster_trading_card_game.CardCollections;
using monster_trading_card_game.Cards;

namespace monster_trading_card_game.Users {
    public interface IUser {
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
        void GenerateCardStack(); 
        CardStack GenerateRandomSpells(int count);
        CardStack GenerateRandomMonsters(int count);
        void Print();
    }
}
