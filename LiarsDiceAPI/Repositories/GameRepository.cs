using System;
using System.Linq;
using LiarsDiceAPI.Models;
using Microsoft.Extensions.Caching.Memory;

namespace LiarsDiceAPI.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly IMemoryCache _cache;
        private const string _allGamesKey = "game_ids";

        public GameRepository(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Game[] GetAllGames()
        {
            Guid[] games;
            if (_cache.TryGetValue<Guid[]>(_allGamesKey, out games))
            {
                return games.Select(id => GetGameById(id)).ToArray();
            }

            return new Game[] { };
        }

        public Game GetGameById(Guid id)
        {
            return _cache.Get<Game>(id);
        }

        public void SaveGame(Game game)
        {
            var ids = _cache.Get<Guid[]>(_allGamesKey);
            ids = ids == null ? new Guid[] { game.Id } : ids.Append(game.Id).ToArray();

            _cache.Set(_allGamesKey, ids);
            _cache.Set(game.Id, game);
        }
    }
}