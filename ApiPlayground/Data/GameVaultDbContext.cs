using Microsoft.EntityFrameworkCore;
namespace ApiPlayground.Data;

public class GameVaultDbContext : DbContext
{
    public GameVaultDbContext(DbContextOptions<GameVaultDbContext> options) : base(options)
    {}
    public DbSet<Game> Games { get; set; }
    public DbSet<Platform> Platforms { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Collection> Collections { get; set; }
    public DbSet<GamePlatform> GamePlatforms { get; set; }
    public DbSet<GameGenre> GameGenres { get; set; }
    public DbSet<CollectionGame> CollectionGames { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<LibraryEntry> LibraryEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GamePlatform>().HasKey(gp => new { gp.GameId, gp.PlatformId });
        modelBuilder.Entity<GameGenre>().HasKey(gg => new { gg.GameId, gg.GenreId });
        modelBuilder.Entity<CollectionGame>().HasKey(cg => new { cg.CollectionId, cg.GameId });
        modelBuilder.Entity<LibraryEntry>().HasIndex(le => new { le.UserId, le.GameId }).IsUnique();
    }

}
    
   

