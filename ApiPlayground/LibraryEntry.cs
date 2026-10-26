namespace ApiPlayground;

public class LibraryEntry
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public User User { get; set; }

    public int GameId { get; set; }

    public Game Game { get; set; }

    public int PlatformID { get; set; }

    public Platform Platform { get; set; }

    public DateOnly? DateStarted { get; set; }

    public DateOnly? DateFinished { get; set; }

    public CompletionStatus CompletionStatus { get; set; }

    public int? PersonalRating { get; set; }

    public string? ReviewText { get; set; }



}
