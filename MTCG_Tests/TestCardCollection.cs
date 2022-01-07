﻿using System.Collections.Generic;
using NUnit.Framework;
using monster_trading_card_game.CardCollections;
using monster_trading_card_game.Cards;
using monster_trading_card_game.Enums;

namespace MTCG_Tests {
	public class TestCardCollection {
		private CardStack _stack;
		private Deck _deck;

		private const int CardStackCountTest = 2;
		private const int CardDeckCountTest = 4;

		[SetUp]
		public void Setup() {
			_stack = new CardStack();
			_deck = new Deck();
		}

		[Test]
		public void TestIsEmpty_ReturnsFalse() {
			// Arrange
			_stack.AddCard(new Spell(0, "Test", 0, ElementType.Water));
			_stack.AddCard(new Spell(0, "Test", 0, ElementType.Water));

			// Assert
			Assert.IsFalse(_stack.IsEmpty());
			Assert.AreEqual(CardStackCountTest, _stack.Count());
		}

		[Test]
		public void TestCount_Returns2() {
			var cardToRemove = new Monster(0, "To Remove", 0, ElementType.Fire, MonsterType.Elf);
			// Arrange
			_stack.AddCard(cardToRemove);
			_stack.AddCard(new Spell(0, "Test", 0, ElementType.Water));
			_stack.AddCard(new Spell(0, "Test", 0, ElementType.Water));
			_stack.RemoveCard(cardToRemove);

			// Assert
			Assert.AreEqual(CardStackCountTest, _stack.Count());
			Assert.IsFalse(_stack.Cards.Contains(cardToRemove));
		}

		[Test]
		public void TestAutoGenerateDeck() {
			// Arrange
			var card1NotIncluded = new Spell(0, "Test", 10, ElementType.Water);
			var card2 = new Monster(0, "Test", 20, ElementType.Water, MonsterType.Dragon);
			var card3 = new Spell(0, "Test", 30, ElementType.Water);
			var card4 = new Monster(0, "Test", 40, ElementType.Water, MonsterType.Orc);
			var card5 = new Spell(0, "Test", 50, ElementType.Water);

			_stack.Cards = new List<ICard>() { card1NotIncluded, card2, card3, card4, card5 };

			// Act
			var result = _deck.AutoCreateDeck(_stack); 

			// Assert
			Assert.AreEqual(CardDeckCountTest, result.Count());
			Assert.Contains(card2, result.Cards);
			Assert.Contains(card3, result.Cards);
			Assert.Contains(card4, result.Cards);
			Assert.Contains(card5, result.Cards);
			Assert.IsFalse(_deck.Cards.Contains(card1NotIncluded));
		}
	}
}