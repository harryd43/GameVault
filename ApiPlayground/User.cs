namespace ApiPlayground;

public class User
{
    public int Id { get; set; }

    public required string Username { get; set; }

    public required string Email { get; set; }

    public string? ProfilePictureUrl { get; set; }

    public string? Bio { get; set; }

    public ICollection<LibraryEntry> LibraryEntries { get; set; } = new List<LibraryEntry>();

    public ICollection<Collection> Collections { get; set; } = new List<Collection>();

}
