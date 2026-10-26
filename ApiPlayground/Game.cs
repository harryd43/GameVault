namespace ApiPlayground;
public class Game
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required DateOnly ReleaseDate { get; set; }

    public required string Developer { get; set; }

    public ICollection<GamePlatform> GamePlatform { get; set; } = new List<GamePlatform>();

    public ICollection<GameGenre> GameGenre { get; set; } = new List<GameGenre>();

}
