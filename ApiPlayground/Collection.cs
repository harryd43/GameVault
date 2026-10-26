namespace ApiPlayground;

public class Collection
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public User User { get; set; }

    public required string Name { get; set; }

    public ICollection<CollectionGame> CollectionGames { get; set; } = new List<CollectionGame>();
}
