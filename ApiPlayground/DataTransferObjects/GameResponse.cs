namespace ApiPlayground.DataTransferObjects
{
    public class GameResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public required string Developer { get; set; }
        public List<PlatformResponse> Platforms { get; set; } = new();
        public List<GenreResponse> Genres { get; set; } = new();
    }
}
