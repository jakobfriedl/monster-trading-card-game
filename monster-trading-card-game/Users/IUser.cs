﻿using System;
using System.Collections.Generic;
using System.Text;
using monster_trading_card_game.CardCollections;

namespace monster_trading_card_game.Users {
    interface IUser {
	    string Username { get; set; }
	    string Password { get; set; }
        int Coins { get; set; }
        int Elo { get; set; }
        int Wins { get; set; }
        int Losses { get; set; }
        Stack CardStack { get; set; }
        Deck Deck { get; set; }

    }
}
