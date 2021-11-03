using System.Linq;
using NUnit.Framework;
using Moq;
using monster_trading_card_game.CardCollections;
using monster_trading_card_game.Cards;
using monster_trading_card_game.Enums;

namespace MTCG_Tests {
	public class TestCardCollection {
		private ICardCollection _stack = new CardStack();

		private const int CardCountTest = 2; 

		[SetUp]
		public void Setup() {
		}

		[Test]
		public void TestIsEmpty_ReturnsFalse() {
			// Arrange
			_stack.AddCard(new Spell("Test", 0, ElementType.Water));

			// Assert
			Assert.IsFalse(_stack.IsEmpty());
		}

		public void TestCount_Returns2() {
			// Arrange
			_stack.AddCard(new Spell("Test", 0, ElementType.Water));
			_stack.AddCard(new Spell("Test", 0, ElementType.Water));
			_stack.AddCard(new Spell("Test", 0, ElementType.Water));
			_stack.RemoveCard(_stack.Cards.ElementAt(0));

			// Assert
			Assert.AreEqual(CardCountTest, _stack.Count());
		}
	}
}