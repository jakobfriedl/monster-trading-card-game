using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monster_trading_card_game.Battle {
    class BattleRequest {
        public int Id { get; set; }
        public int User1 { get; set; }
        public int User2 { get; set; }
        public bool Completed { get; set; }

        public BattleRequest(int id, int user1, int user2, bool completed) {
	        Id = id;
	        User1 = user1;
	        User2 = user2;
	        Completed = completed; 
        }

        public BattleRequest(int user1, int user2) {
	        Id = 0;
	        User1 = user1;
	        User2 = user2;
	        Completed = false; 
        }
    }
}
