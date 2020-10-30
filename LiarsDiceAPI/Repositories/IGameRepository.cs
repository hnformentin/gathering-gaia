using System;
using LiarsDiceAPI.Models;

namespace LiarsDiceAPI.Repositories
{
    public interface IGameRepository
    {
        Game[] GetAllGames();
        Game GetGameById(Guid id);
        void SaveGame(Game game);
    }
}