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
	    private Battle battle; 

        [SetUp]
        public void Setup() {
	        battle = new Battle(user1.Object, user2.Object); 
        }

        [Test]
        public void TestGoblinVsDragon_ReturnsDragon() {
            // Arrange
	        IMonster goblin = new Monster("Goblin", 10, ElementType.Normal, MonsterType.Goblin);
	        IMonster dragon = new Monster("Dragon", 10, ElementType.Normal, MonsterType.Dragon);
	        
            // Act
	        ICard winner = battle.MonsterBattle(goblin, dragon);
	        
            // Assert
            Assert.True(winner == dragon);
        }
		[Test]
        public void TestWizardVsOrc_ReturnsWizard() {
	        // Arrange
	        IMonster wizard = new Monster("Wizard", 10, ElementType.Normal, MonsterType.Wizard);
	        IMonster orc = new Monster("Orc", 10, ElementType.Normal, MonsterType.Orc);

	        // Act
	        ICard winner = battle.MonsterBattle(orc, wizard);

	        // Assert
	        Assert.True(winner == wizard);
        }

        [Test]
        public void TestFireElfVsStrongerDragon_ReturnsElf() {
	        IMonster elf = new Monster("Elf", 10, ElementType.Fire, MonsterType.Elf);
	        IMonster dragon = new Monster("Dragon", 20, ElementType.Normal, MonsterType.Dragon);

	        // Act
	        ICard winner = battle.MonsterBattle(elf, dragon);

	        // Assert
	        Assert.True(winner == elf);
		}

        [Test]
        public void TestRegElfVsStrongerDragon_ReturnsDragon() {
	        IMonster elf = new Monster("Elf", 10, ElementType.Normal, MonsterType.Elf);
	        IMonster dragon = new Monster("Dragon", 20, ElementType.Normal, MonsterType.Dragon);

	        // Act
	        ICard winner = battle.MonsterBattle(elf, dragon);

	        // Assert
	        Assert.True(winner == dragon);
        }

        [Test]
        public void TestDraw_ReturnsNull() {
	        IMonster goblin = new Monster("Goblin", 10, ElementType.Normal, MonsterType.Goblin);
	        IMonster knight = new Monster("Knight", 10, ElementType.Normal, MonsterType.Knight);

	        // Act
	        ICard winner = battle.MonsterBattle(goblin, knight);

	        // Assert
	        Assert.IsNull(winner);
        }

        [Test]
        public void TestKrakenVsStrongerDragon_ReturnsDragon(){
	        IMonster kraken = new Monster("Kraken", 10, ElementType.Normal, MonsterType.Kraken);
	        IMonster dragon = new Monster("Dragon", 50, ElementType.Normal, MonsterType.Dragon);

	        // Act
	        ICard winner = battle.MonsterBattle(kraken, dragon);

	        // Assert
	        Assert.True(winner == dragon);
        }
	}
}