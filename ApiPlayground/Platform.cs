namespace ApiPlayground;

public class Platform
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public ICollection<GamePlatform> GamePlatforms { get; set; } = new List<GamePlatform>();
}
