using System;
using System.Collections.Generic;
using System.Linq;
using LiarsDiceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LiarsDiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private IMemoryCache _cache;
        public GamesController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        // GET: api/<GamesController>
        [HttpGet]
        [Produces(typeof(IEnumerable<GameInfo>))]
        public ActionResult<IEnumerable<GameInfo>> Get()
        {
            return Ok(new Game[] { });
        }

        // GET api/<GamesController>/5
        [HttpGet("{id}")]
        [Produces(typeof(Game))]
        public ActionResult<Game> Get(Guid id)
        {
            var game = GetGameById(id);
            return game;
        }

        // POST api/<GamesController>
        // Why you cannot use string gameName here:
        // https://briancaos.wordpress.com/2019/11/12/asp-net-core-api-s-is-an-invalid-start-of-a-value-path-linenumber-0-bytepositioninline-0/
        [HttpPost]
        [Produces(typeof(Game))]
        public ActionResult<Game> Post(object gameName)
        {

            var game = new Game(gameName.ToString());
            SaveGame(game);
            return game;
        }

        [HttpPut("{id}/players")]
        public Guid Join(Guid id, [FromBody] string username)
        {
            var game = GetGameById(id);
            var player = game.JoinGame(username);
            return player.UserId;
        }

        [HttpPut("{id}/start")]
        public void Start(Guid id, [FromBody] Guid userId)
        {
            var game = GetGameById(id);
            game.StartGame();
        }

        [HttpPut("{id}/call")]
        public void Call(Guid id, [FromBody] Guid userId)
        {

        }

        [HttpPut("{id}/bid")]
        public void Call(Guid id, [FromBody] Bid bid)
        {

        }

        // DELETE api/<GamesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok();
        }

        private Game GetGameById(Guid id)
        {
            return _cache.Get<Game>(id);
        }

        private void SaveGame(Game game)
        {
            var ids = _cache.Get<string[]>("games");

            _cache.Set(game.Id, game);
        }
    }
}
