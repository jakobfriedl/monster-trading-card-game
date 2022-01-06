using System;
using NUnit.Framework;
using Moq;
using monster_trading_card_game.Security;
using NUnit.Framework.Internal;

namespace MTCG_Tests {
    class TestPasswordHasher {
	    private PasswordHasher _hasher;
	    private const string _password = "Password123"; 
		
	    [SetUp]
		public void Setup() {
			_hasher = new PasswordHasher(); 
		}

		[Test]
		public void TestHash() {
			// Arrange
			var hash = _hasher.Hash(_password);

			// Assert
			Console.WriteLine(hash);
			Assert.IsTrue(hash.StartsWith("$MTCGHASH$10000$"));
		}

		[Test]
		public void TestHashVerify() {
			// Arrange
			var hash = _hasher.Hash(_password);

			// Assert
			Assert.IsTrue(_hasher.Verify(_password, hash));
		}

		[Test]
		public void TestIsHashSupported() {
			// Arrange
			var invalidHash = "$INVALIDHASH$1000$ThisIsAInvalidHash"; 
			var validHash = "$MTCGHASH$10000$ThisIsAValidHash"; 

			// Assert
			Assert.IsTrue(_hasher.IsHashSupported(validHash));
			Assert.IsFalse(_hasher.IsHashSupported(invalidHash));
		}
    }
}
