using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monster_trading_card_game.Trade {
    public class Offer {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CardId { get; set; }
        public int Price { get; set; }

        public Offer(int id, int user, int card, int price) {
	        Id = id; 
	        UserId = user;
	        CardId = card;
	        Price = price <= 0 ? 5 : price; 
        }

        public void Print() {

        }
    }
}
