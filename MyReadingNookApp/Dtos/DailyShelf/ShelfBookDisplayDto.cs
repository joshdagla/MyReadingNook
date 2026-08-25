namespace MyReadingNookApp.Dtos.DailyShelf;

public class ShelfBookDisplayDto
{
    public int Isbn { get; set; }

    public string? Title { get; set; }
    
    public string? Author { get; set; }

    public string? ImagePath { get; set; }
}