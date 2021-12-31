using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using monster_trading_card_game.Database;
using monster_trading_card_game.Enums;

namespace monster_trading_card_game.Trade {
    public class Offer {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CardId { get; set; }
        public int Element { get; set; }
        public int Monster { get; set; }
        public int MinDamage { get; set; }
        public int Price { get; set; }

        public Offer(int id, int user, int card, int element, int monster, int damage, int price) {
	        Id = id; 
	        UserId = user;
	        CardId = card;
	        Element = element;
	        Monster = monster;
	        MinDamage = damage; 
	        Price = price <= 0 ? 5 : price; 
        }

        public void PrintOwn() {
	        var dbCard = new DBCard();

	        var card = dbCard.GetCardByCardId(CardId);
	        var element = Element<=-1 ? "Any" : ((ElementType)Element).ToString(); 
	        var cardType = Monster<=-1 ? "Any" : Monster == 0 ? "Spell" : ((MonsterType)Monster).ToString(); 

	        card.PrintCardName();
	        Console.WriteLine($"{card.Damage.ToString().PadRight(10)}{element.PadRight(9)}{cardType.PadRight(12)}{MinDamage.ToString().PadRight(12)}{Price.ToString().PadRight(7)}");
        }

        public void PrintOther() {
	        var dbCard = new DBCard();
	        var dbUser = new DBUser(); 

			var card = dbCard.GetCardByCardId(CardId);
	        var user = dbUser.GetUsernameByUserId(UserId);
	        var element = Element <= -1 ? "Any" : ((ElementType)Element).ToString();
	        var cardType = Monster <= -1 ? "Any" : Monster == 0 ? "Spell" : ((MonsterType)Monster).ToString();

			Console.Write(user.PadRight(15));
	        card.PrintCardName();
			Console.WriteLine($"{card.Damage.ToString().PadRight(10)}{element.PadRight(9)}{cardType.PadRight(12)}{MinDamage.ToString().PadRight(12)}{Price.ToString().PadRight(7)}");
        }
	}
}
