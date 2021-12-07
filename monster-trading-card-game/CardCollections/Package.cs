﻿using System;
using System.Collections.Generic;
using System.Text;
using Castle.Core.Internal;
using monster_trading_card_game.Cards;

namespace monster_trading_card_game.CardCollections {
    public class Package : ICardCollection {
	    public int Capacity { get; } = 5;
	    public List<ICard> Cards { get; set; }

	    public Package() {
		    Cards = new List<ICard>(Capacity);
	    }

	    public void AddCard(ICard card) {
		    if (Cards.Count < Capacity) {
			    Cards.Add(card);
		    }
	    }

	    public void Print() {
		    foreach (ICard card in Cards) {
			    Console.WriteLine($"  {card.Id} -- {card.Name} - {card.Damage}");
			}
		}

	    public void RemoveCard(ICard card) {
		    Cards.Remove(card);
		}

	    public int Count() {
		    return Cards.Count;
	    }

	    public bool IsEmpty() {
		    return Cards.IsNullOrEmpty();
	    }
	}
}
