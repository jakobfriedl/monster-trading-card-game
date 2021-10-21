using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using NUnit.Framework;
using Moq;
using monster_trading_card_game.Cards;
using monster_trading_card_game.Enums;
using monster_trading_card_game.Users;
using NUnit.Framework.Internal;

namespace mtcgTests {
    public class TestBattle {
	    private Mock<IUser> user1 = new Mock<IUser>();
	    private Mock<IUser> user2 = new Mock<IUser>();
	    private Battle _battle; 

        [SetUp]
        public void Setup() {
	        _battle = new Battle(user1.Object, user2.Object); 
        }

		////////// Test MonsterBattle Function //////////////
		[Test]
        public void TestMonsterBattleGoblinVsDragon_ReturnsDragon() {
            // Arrange
	        ICard goblin = new Monster("Goblin", 10, ElementType.Normal, MonsterType.Goblin);
	        ICard dragon = new Monster("Dragon", 10, ElementType.Normal, MonsterType.Dragon);
	        
            // Act
	        ICard winner = _battle.MonsterBattle(goblin, dragon);
	        
            // Assert
            Assert.True(winner == dragon);
        }
		[Test]
        public void TestMonsterBattleWizardVsOrc_ReturnsWizard() {
			// Arrange
			ICard wizard = new Monster("Wizard", 10, ElementType.Normal, MonsterType.Wizard);
			ICard orc = new Monster("Orc", 10, ElementType.Normal, MonsterType.Orc);

	        // Act
	        ICard winner = _battle.MonsterBattle(orc, wizard);

	        // Assert
	        Assert.True(winner == wizard);
        }

        [Test]
        public void TestMonsterBattleFireElfVsStrongerDragon_ReturnsElf() {
			//Arrange
			ICard elf = new Monster("Elf", 10, ElementType.Fire, MonsterType.Elf);
			ICard dragon = new Monster("Dragon", 20, ElementType.Normal, MonsterType.Dragon);

	        // Act
	        ICard winner = _battle.MonsterBattle(elf, dragon);

	        // Assert
	        Assert.True(winner == elf);
		}

        [Test]
        public void TestMonsterBattleRegElfVsStrongerDragon_ReturnsDragon() {
			//Arrange
			ICard elf = new Monster("Elf", 10, ElementType.Normal, MonsterType.Elf);
			ICard dragon = new Monster("Dragon", 20, ElementType.Normal, MonsterType.Dragon);

	        // Act
	        ICard winner = _battle.MonsterBattle(elf, dragon);

	        // Assert
	        Assert.True(winner == dragon);
        }

        [Test]
        public void TestMonsterBattleDraw_ReturnsNull() {
			//Arrange
			ICard goblin = new Monster("Goblin", 10, ElementType.Normal, MonsterType.Goblin);
			ICard knight = new Monster("Knight", 10, ElementType.Normal, MonsterType.Knight);

	        // Act
	        ICard winner = _battle.MonsterBattle(goblin, knight);

	        // Assert
	        Assert.IsNull(winner);
        }

        [Test]
        public void TestKrakenVsStrongerDragon_ReturnsDragon(){
			//Arrange
			ICard kraken = new Monster("Kraken", 10, ElementType.Normal, MonsterType.Kraken);
			ICard dragon = new Monster("Dragon", 50, ElementType.Normal, MonsterType.Dragon);

	        // Act
	        ICard winner = _battle.MonsterBattle(kraken, dragon);

	        // Assert
	        Assert.True(winner == dragon);
        }

        ////////// Test SpellBattle Function //////////////
        [Test]
        public void TestSpellBattleFire10VsWater20_ReturnsWater() {
			//Arrange
	        ICard water = new Spell("Water Spell", 20, ElementType.Water);
	        ICard fire = new Spell("Fire Spell", 10, ElementType.Fire);

			// Act
			ICard winner = _battle.SpellBattle(water, fire);

			// Assert
			Assert.That(winner == water);
        }

        [Test]
        public void TestSpellBattleFire20VsWater5_ReturnsNull() {
	        //Arrange
	        ICard water = new Spell("Water Spell", 5, ElementType.Water);
	        ICard fire = new Spell("Fire Spell", 20, ElementType.Fire);

	        // Act
	        ICard winner = _battle.SpellBattle(water, fire);

	        // Assert
	        Assert.IsNull(winner);
        }

        [Test]
        public void TestSpellBattleFire90VsWater5_ReturnsFire() {
	        //Arrange
	        ICard water = new Spell("Water Spell", 5, ElementType.Water);
	        ICard fire = new Spell("Fire Spell", 90, ElementType.Fire);

	        // Act
	        ICard winner = _battle.SpellBattle(water, fire);

	        // Assert
	        Assert.True(winner == fire);
        }

		[Test]
        public void TestSpellBattleFire10VsNormal20_ReturnsFire() {
	        //Arrange
	        ICard normal = new Spell("Normal Spell", 20, ElementType.Normal);
	        ICard fire = new Spell("Fire Spell", 10, ElementType.Fire);

	        // Act
	        ICard winner = _battle.SpellBattle(normal, fire);

	        // Assert
	        Assert.That(winner == fire);
        }

        [Test]
        public void TestSpellBattleFire5VsNormal20_ReturnsNull() {
			//Arrange
			ICard normal = new Spell("Normal Spell", 20, ElementType.Normal);
			ICard fire = new Spell("Fire Spell", 5, ElementType.Fire);

			// Act
			ICard winner = _battle.SpellBattle(normal, fire);

	        // Assert
	        Assert.IsNull(winner);
        }

        [Test]
        public void TestSpellBattleFire20VsNormal100_ReturnsNormal() {
			//Arrange
			ICard normal = new Spell("Normal Spell", 100, ElementType.Normal);
			ICard fire = new Spell("Fire Spell", 20, ElementType.Fire);

			// Act
			ICard winner = _battle.SpellBattle(normal, fire);

	        // Assert
	        Assert.True(winner == normal);
        }

        [Test]
        public void TestSpellBattleNormal10VsWater20_ReturnsNormal() {
	        //Arrange
	        ICard normal = new Spell("Normal Spell", 10, ElementType.Normal);
	        ICard water = new Spell("Water Spell", 20, ElementType.Water);

	        // Act
	        ICard winner = _battle.SpellBattle(normal, water);

	        // Assert
	        Assert.That(winner == normal);
        }

        [Test]
        public void TestSpellBattleNormal10VsWater40_ReturnsNull() {
			//Arrange
			ICard normal = new Spell("Normal Spell", 10, ElementType.Normal);
			ICard water = new Spell("Water Spell", 40, ElementType.Water);

			// Act
			ICard winner = _battle.SpellBattle(normal, water);

			// Assert
			Assert.IsNull(winner);
        }

        [Test]
        public void TestSpellBattleNormal10VsWater50_ReturnsWater() {
	        //Arrange
	        ICard normal = new Spell("Normal Spell", 10, ElementType.Normal);
	        ICard water = new Spell("Water Spell", 50, ElementType.Water);

	        // Act
	        ICard winner = _battle.SpellBattle(normal, water);

	        // Assert
	        Assert.True(winner == water);
        }

        [Test]
        public void TestSpellBattleWater100VsWater80_ReturnsWater100() {
	        //Arrange
	        ICard water100 = new Spell("Water Spell", 100, ElementType.Water);
	        ICard water80 = new Spell("Water Spell", 80, ElementType.Water);

	        // Act
	        ICard winner = _battle.SpellBattle(water80, water100);

	        // Assert
	        Assert.True(winner == water100);
		}

        ////////// Test MixedBattle Function //////////////
        [Test]
        public void TestMixedBattleFireSpellVsWaterGoblin_ReturnsGoblin() {
			// Arrange 
	        ICard goblin = new Monster("Water Goblin", 10, ElementType.Water, MonsterType.Goblin);
	        ICard fire = new Spell("Fire Spell", 10, ElementType.Fire); 

			// Act
			ICard winner = _battle.MixedBattle(goblin, fire); 

			// Assert
			Assert.True(winner == goblin);
        }

        [Test]
        public void TestMixedBattleWaterSpellVsWaterGoblin_ReturnsNull() {
	        // Arrange 
	        ICard goblin = new Monster("Water Goblin", 10, ElementType.Water, MonsterType.Goblin);
	        ICard water = new Spell("Water Spell", 10, ElementType.Water);

	        // Act
	        ICard winner = _battle.MixedBattle(goblin, water);

	        // Assert
	        Assert.IsNull(winner);
        }

        [Test]
        public void TestMixedBattleRegularSpellVsKraken_ReturnsKraken() {
	        // Arrange 
	        ICard kraken = new Monster("Fire Kraken", 50, ElementType.Fire, MonsterType.Kraken);
	        ICard normal = new Spell("Normal Spell", 100, ElementType.Normal);

	        // Act
	        ICard winner = _battle.MixedBattle(kraken, normal);

	        // Assert
	        Assert.True(winner == kraken);
        }

        [Test]
        public void TestMixedBattleRegularSpellVsRegularKnight_ReturnsKnight() {
			// Arrange 
			ICard knight = new Monster("Normal Knight", 15, ElementType.Normal, MonsterType.Knight);
			ICard normal = new Spell("Normal Spell", 10, ElementType.Normal);

			// Act
			ICard winner = _battle.MixedBattle(knight, normal);

			// Assert
			Assert.True(winner == knight);
		}

        [Test]
        public void TestMixedBattleWaterSpellVsRegularKnight_ReturnsWater() {
	        // Arrange 
	        ICard knight = new Monster("Normal Knight", 100, ElementType.Normal, MonsterType.Knight);
	        ICard water = new Spell("Water Spell", 10, ElementType.Water);

	        // Act
	        ICard winner = _battle.MixedBattle(knight, water);

	        // Assert
	        Assert.True(winner == water);
        }
	}
}