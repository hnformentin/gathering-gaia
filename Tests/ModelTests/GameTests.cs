using System.Linq;
using LiarsDiceAPI.Models;
using NUnit.Framework;

namespace Tests.ModelTests
{
    public class GameTests
    {
        [TestFixture]
        public class When_Creating_A_Game
        {
            [Test]
            public void Assure_Game_Status_Is_NotStarted()
            {
                var game = new Game("En game");
                Assert.That(game.Status, Is.EqualTo(GameStatus.NotStarted));
            }
        }

        [TestFixture]
        public class When_starting_a_game
        {
            [Test]
            public void Assure_more_than_one_player()
            {
                var game = new Game("et game");
                Assert.That(() => game.StartGame(), Throws.InvalidOperationException);
            }

            [Test]
            public void Assure_game_status_is_running()
            {
                var game = StartGame();

                Assert.That(game.Status, Is.EqualTo(GameStatus.Running));
            }

            [Test]
            public void Assure_all_players_has_starting_number_of_dices()
            {
                var game = StartGame();

                Assert.That(game.ActivePlayers.All(player => player.DiceCount == Game.InitialDiceCount), Is.True);
            }

            [Test]
            public void Assure_has_current_player()
            {
                var game = StartGame();

                Assert.That(game.CurrentPlayer, Is.Not.Null);
            }

            [Test]
            public void Assure_cannot_start_already_running_game()
            {
                var game = StartGame();

                Assert.That(() => game.StartGame(), Throws.InvalidOperationException);
            }

            [Test]
            public void Assure_only_game_master_can_start_game()
            {

            }

            private Game StartGame()
            {

                var game = new Game("et game");
                game.JoinGame("player 1");
                game.JoinGame("player 2");
                game.StartGame();
                return game;
            }
        }

        [TestFixture]
        public class When_joining_a_game
        {
            [Test]
            public void Assure_player_is_added()
            {
                var game = new Game("En game");
                game.JoinGame("My name");

                Assert.That(game.Players.FirstOrDefault().UserName, Is.EqualTo("My name"));
            }

            [Test]
            public void Assure_cant_join_game_with_max_players()
            {
                var game = new Game("En game");
                game.JoinGame("My name");
                game.JoinGame("My name 2");
                game.JoinGame("My name 3");
                game.JoinGame("My name 4");

                Assert.That(() => game.JoinGame("My name 5"), Throws.InvalidOperationException);
            }

            [Test]
            public void Assure_player_username_must_be_unique()
            {
                var game = new Game("En game");
                game.JoinGame("My name");
                Assert.That(() => game.JoinGame("My name"), Throws.InvalidOperationException);
            }

            [Test]
            public void Assure_player_username_not_empty()
            {
                var game = new Game("En game");
                Assert.That(() => game.JoinGame(null), Throws.InvalidOperationException);
                Assert.That(() => game.JoinGame(""), Throws.InvalidOperationException);
                Assert.That(() => game.JoinGame("  "), Throws.InvalidOperationException);
            }
        }
    }
}
