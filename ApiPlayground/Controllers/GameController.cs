using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ApiPlayground.Data;
using ApiPlayground.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace ApiPlayground.Controllers
{
    [ApiController]
    [Route("/api/games")]
    public class GameController : ControllerBase
    {
        private readonly GameVaultDbContext _context;

        public GameController(GameVaultDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<GameResponse> Get()
        {
            return _context.Games
                .Include(g => g.GamePlatform)
                .ThenInclude(gp => gp.Platform)
                .Include(g => g.GameGenre)
                .ThenInclude(gg => gg.Genre)
                .AsEnumerable()
                .Select(ToGameResponse)
                .ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<GameResponse> GetById(int id)
        {
            var game = _context.Games
                .Include(g => g.GamePlatform)
                .ThenInclude(gp => gp.Platform)
                .Include(g => g.GameGenre)
                .ThenInclude(gg => gg.Genre)
                .FirstOrDefault(g => g.Id == id);
            if (game == null)
            {
                return NotFound("Game not found.");
            }

            return ToGameResponse(game);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var game = _context.Games.Find(id);
            if (game == null)
            {
                return NotFound();
            }

            _context.Games.Remove(game);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{gameId}/platforms/{platformId}")]
        public ActionResult RemovePlatform(int gameId, int platformId)
        {
            var gamePlatform = _context.GamePlatforms
                .FirstOrDefault(gp => gp.GameId == gameId && gp.PlatformId == platformId);
            if (gamePlatform == null)
            {
                return NotFound("Association not found.");
            }
            _context.GamePlatforms.Remove(gamePlatform);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{gameId}/genres/{genreId}")]
        public ActionResult RemoveGenre(int gameId, int genreId)
        {
            var gameGenre = _context.GameGenres
                .FirstOrDefault(gg => gg.GameId == gameId && gg.GenreId == genreId);
            if (gameGenre == null)
            {
                return NotFound("Association not found.");
            }
            _context.GameGenres.Remove(gameGenre);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPost]
        public ActionResult<GameResponse> Create(CreateGameRequest request)
        {
            if (request.ReleaseDate > DateOnly.FromDateTime(DateTime.Today))
            {
                return BadRequest("Release date cannot be in the future.");
            }
            var game = new Game
            {
                Name = request.Name,
                Developer = request.Developer,
                ReleaseDate = request.ReleaseDate
            };

            _context.Games.Add(game);
            _context.SaveChanges();

            var response = ToGameResponse(game);

            return CreatedAtAction(nameof(GetById), new { id = game.Id }, response);
        }

        [HttpPost("{gameId}/platforms/{platformId}")]
        public ActionResult AddPlatform(int gameId, int platformId)
        {
            var game = _context.Games.Find(gameId);
            if (game == null)
            {
                return NotFound("Game not found.");
            }
            var platform = _context.Platforms.Find(platformId);
            if (platform == null)
            {
                return NotFound("Platform not found.");
            }
            if (_context.GamePlatforms.Any(gp => gp.GameId == gameId && gp.PlatformId == platformId))
            {
                return Conflict("Platform is already associated with this game.");
            }
            var gamePlatform = new GamePlatform
            {
                GameId = gameId,
                PlatformId = platformId
            };
            _context.GamePlatforms.Add(gamePlatform);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPost("{gameId}/genres/{genreId}")]
        public ActionResult AddGenre(int gameId, int genreId)
        {
            var game = _context.Games.Find(gameId);
            if (game == null)
            {
                return NotFound("Game not found.");
            }
            var genre = _context.Genres.Find(genreId);
            if (genre == null)
            {
                return NotFound("Genre not found.");
            }
            if (_context.GameGenres.Any(gg => gg.GameId == gameId && gg.GenreId == genreId))
            {
                return Conflict("Genre is already associated with this game.");
            }
            var gameGenre = new GameGenre
            {
                GameId = gameId,
                GenreId = genreId
            };
            _context.GameGenres.Add(gameGenre);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id}")]
        public ActionResult<GameResponse> Update(int id, CreateGameRequest request)
        {
            var game = _context.Games
                .Include(g => g.GamePlatform)
                .ThenInclude(gp => gp.Platform)
                .Include(g => g.GameGenre)
                .ThenInclude(gg => gg.Genre)
                .FirstOrDefault(g => g.Id == id);
            if (game == null)
            {
                return NotFound("Game not found.");
            }
            if (request.ReleaseDate > DateOnly.FromDateTime(DateTime.Today))
            {
                return BadRequest("Release date cannot be in the future.");
            }
            game.Name = request.Name;
            game.Developer = request.Developer;
            game.ReleaseDate = request.ReleaseDate;
            _context.SaveChanges();
            var response = ToGameResponse(game);
            return Ok(response);
        }

        private GameResponse ToGameResponse(Game game)
        {
            return new GameResponse
            {
                Id = game.Id,
                Name = game.Name,
                ReleaseDate = game.ReleaseDate,
                Developer = game.Developer,
                Platforms = game.GamePlatform.Select(gp => new PlatformResponse
                {
                    Id = gp.Platform.Id,
                    Name = gp.Platform.Name
                }).ToList(),
                Genres = game.GameGenre.Select(gg => new GenreResponse
                {
                    Id = gg.Genre.Id,
                    Name = gg.Genre.Name
                }).ToList()
            };
        }
    }
}