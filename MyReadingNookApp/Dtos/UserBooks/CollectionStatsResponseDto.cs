namespace MyReadingNookApp.Dtos.UserBooks;

public class CollectionItemDto
{
    public string CollectionName { get; set; } = string.Empty;
    public int BookCount { get; set; }
}

public class CollectionStatsResponseDto
{
    public int TotalCollections { get; set; }

    public List<CollectionItemDto> Collections { get; set; } = new();
}
