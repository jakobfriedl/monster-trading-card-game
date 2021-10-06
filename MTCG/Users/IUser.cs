using System;
using System.Collections.Generic;
using System.Text;
using MTCG.CardCollections;

namespace MTCG.Users {
    interface IUser {
	    string Username { get; set; }
	    string Password { get; set; }
        int Coins { get; set; }
        Stack CardStack { get; set; }
        Deck Deck { get; set; }
        int Elo { get; set; }
    }
}
