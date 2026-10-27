using Microsoft.AspNetCore.Mvc;
using ApiPlayground.Data;
using ApiPlayground.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace ApiPlayground.Controllers
{
    [ApiController]
    [Route("api/platforms")]
    public class PlatformController : ControllerBase
    {
        private readonly GameVaultDbContext _context;

        public PlatformController(GameVaultDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<PlatformResponse> Get()
        {
            return _context.Platforms.Select(ToPlatformResponse)
            .ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<PlatformResponse> GetById(int id)
        {
            var platform = _context.Platforms.Find(id);
            if (platform == null)
            {
                return NotFound();
            }
            return ToPlatformResponse(platform);
        }

        [HttpPost]
        public ActionResult<PlatformResponse> Create(CreatePlatformRequest request)
        {
            var platform = new Platform
            {
                Name = request.Name.Trim() 
            };

            if (string.IsNullOrWhiteSpace(platform.Name))
            {
                return BadRequest("Platform name cannot be empty.");
            }

            // Check if a platform with the same name already exists
            if (_context.Platforms.Any(p => EF.Functions.Collate(p.Name, "NOCASE") == platform.Name))
            {
                return Conflict("A platform with the same name already exists.");
            }

            _context.Platforms.Add(platform);
            _context.SaveChanges();

            var response = ToPlatformResponse(platform);

            return CreatedAtAction(nameof(GetById), new { id = platform.Id }, response);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var platform = _context.Platforms.Find(id);
            if (platform == null)
            {
                return NotFound();
            }

            if (_context.LibraryEntries.Any(le => le.PlatformID == id))
            {
                return Conflict("Cannot delete platform because it is associated with one or more library entries.");
            }

            _context.Platforms.Remove(platform);
            _context.SaveChanges();

            return NoContent();
        }

        private PlatformResponse ToPlatformResponse(Platform platform)
        {
            return new PlatformResponse
            {
                Id = platform.Id,
                Name = platform.Name
            };
        }

    }
}
