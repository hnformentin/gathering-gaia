using LiarsDiceAPI.Models;
using NUnit.Framework;

namespace Tests.ModelTests
{
    public class PlayerTests
    {
        [TestFixture]
        public class When_creating_player
        {
            [Test]
            public void Assure_name_is_set()
            {
                var player = new Player("My name");

                Assert.That(player.UserName, Is.EqualTo("My name"));
            }

            [Test]
            public void Assure_guid_is_set()
            {
                var player = new Player("My name");
                Assert.That(player.UserId, Is.Not.Null);
            }
        }

        [TestFixture]
        public class When_checking_if_player_have_lost
        {
            [Test]
            public void Assure_returns_false_when_number_of_dice_is_more_than_one()
            {
                var player = new Player("My name");

                Assert.That(player.HasLost, Is.False);
            }

            [Test]
            public void Assure_returns_true_when_number_of_dice_is_one_or_less()
            {
                var player = new Player("My name");
                for (int i = 0; i < Game.InitialDiceCount-1; i++)
                {
                    player.RemoveDice();
                }

                Assert.That(player.HasLost);
            }
        }

    }
}
