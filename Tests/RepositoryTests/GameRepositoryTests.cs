using System;
using LiarsDiceAPI.Models;
using LiarsDiceAPI.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Tests.RepositoryTests
{
    public class GameRepositoryTests
    {
        [TestFixture]
        public class When_saving_game
        {
            private IServiceProvider _services;

            [SetUp]
            public void Setup()
            {
                var services = new ServiceCollection();
                services.AddMemoryCache();
                services.AddScoped<IGameRepository, GameRepository>();
                _services = services.BuildServiceProvider();
            }

            [Test]
            public void Assure_game_is_saved()
            {
                var repo = _services.GetService<IGameRepository>();
                var game = new Game("my game");

                repo.SaveGame(game);
                var fetchGame = repo.GetGameById(game.Id);

                Assert.That(fetchGame, Is.Not.Null);
                Assert.That(fetchGame.Id, Is.EqualTo(game.Id));
            }

            [Test]
            public void Assure_get_non_existing_game_returns_null()
            {
                var repo = _services.GetService<IGameRepository>();
                var game = repo.GetGameById(Guid.NewGuid());

                Assert.That(game, Is.Null);
            }

            [Test]
            public void Assure_get_all_games_return_empty_array()
            {
                var repo = _services.GetService<IGameRepository>();
                var games = repo.GetAllGames();

                Assert.That(games.Length, Is.EqualTo(0));
            }

            [Test]
            public void Assure_get_all_games_return_array()
            {
                var repo = _services.GetService<IGameRepository>();
                repo.SaveGame(new Game("my game 1"));
                repo.SaveGame(new Game("my game 2"));
                var games = repo.GetAllGames();

                Assert.That(games.Length, Is.EqualTo(2));
                Assert.That(games[0], Is.Not.EqualTo(games[1]));
                Assert.That(games[0].Id, Is.Not.EqualTo(games[1].Id));
            }
        }
    }
}