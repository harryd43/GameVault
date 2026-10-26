using Microsoft.AspNetCore.Mvc;
using ApiPlayground.Data;
using ApiPlayground.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace ApiPlayground.Controllers
{
    [ApiController]
    [Route("api/genres")]
    public class GenreController : ControllerBase
    {
        private readonly GameVaultDbContext _context;

        public GenreController(GameVaultDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<GenreResponse> Get()
        {
            return _context.Genres.Select(ToGenreResponse)
            .ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<GenreResponse> GetById(int id)
        {
            var genre = _context.Genres.Find(id);
            if (genre == null)
            {
                return NotFound();
            }
            return ToGenreResponse(genre);
        }

        [HttpPost]
        public ActionResult<GenreResponse> Create(CreateGenreRequest request)
        {
            var genre = new Genre
            {
                Name = request.Name.Trim()
            };

            if (string.IsNullOrWhiteSpace(genre.Name))
            {
                return BadRequest("Genre name cannot be empty.");
            }

            // Check if a genre with the same name already exists
            if (_context.Genres.Any(g => EF.Functions.Collate(g.Name, "NOCASE") == genre.Name))
            {
                return Conflict("A genre with the same name already exists.");
            }

            _context.Genres.Add(genre);
            _context.SaveChanges();

            var response = ToGenreResponse(genre);

            return CreatedAtAction(nameof(GetById), new { id = genre.Id }, response);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var genre = _context.Genres.Find(id);
            if (genre == null)
            {
                return NotFound();
            }

            _context.Genres.Remove(genre);
            _context.SaveChanges();

            return NoContent();
        }

        private GenreResponse ToGenreResponse(Genre genre)
        {
            return new GenreResponse
            {
                Id = genre.Id,
                Name = genre.Name
            };
        }

    }
}
