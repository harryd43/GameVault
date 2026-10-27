using System.Linq.Expressions;
using ApiPlayground.Data;
using ApiPlayground.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiPlayground.Controllers
{
    [ApiController]
    [Route("api/platforms")]
    public class PlatformController : ControllerBase
    {
        private static readonly Expression<Func<Platform, PlatformResponse>> ToResponse =
            p => new PlatformResponse { Id = p.Id, Name = p.Name };

        private static readonly Func<Platform, PlatformResponse> MapToResponse = ToResponse.Compile();

        private readonly GameVaultDbContext _context;

        public PlatformController(GameVaultDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<PlatformResponse>> Get(CancellationToken cancellationToken)
        {
            return await _context.Platforms
                .Select(ToResponse)
                .ToListAsync(cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlatformResponse>> GetById(int id, CancellationToken cancellationToken)
        {
            var platform = await _context.Platforms
                .Where(p => p.Id == id)
                .Select(ToResponse)
                .FirstOrDefaultAsync(cancellationToken);

            if (platform == null)
            {
                return NotFound();
            }

            return platform;
        }

        [HttpPost]
        public async Task<ActionResult<PlatformResponse>> Create(CreatePlatformRequest request, CancellationToken cancellationToken)
        {
            var platform = new Platform
            {
                Name = request.Name.Trim()
            };

            if (string.IsNullOrWhiteSpace(platform.Name))
            {
                return BadRequest("Platform name cannot be empty.");
            }

            // Case-insensitive duplicate check
            if (await _context.Platforms.AnyAsync(p => EF.Functions.Collate(p.Name, "NOCASE") == platform.Name, cancellationToken))
            {
                return Conflict("A platform with the same name already exists.");
            }

            _context.Platforms.Add(platform);
            await _context.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = platform.Id }, MapToResponse(platform));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var platform = await _context.Platforms.FindAsync([id], cancellationToken);
            if (platform == null)
            {
                return NotFound();
            }

            if (await _context.LibraryEntries.AnyAsync(le => le.PlatformID == id, cancellationToken))
            {
                return Conflict("Cannot delete platform because it is associated with one or more library entries.");
            }

            _context.Platforms.Remove(platform);
            await _context.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}