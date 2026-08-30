namespace MyReadingNookApp.Dtos.UserBooks;

public class BookItemDto
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    
    public string Author { get; set; } = string.Empty;
    
    public string ImagePath { get; set; } = string.Empty;
}


public class CurrentBooksResponseDto
{
    public int CurrentBooksCount { get; set; } = 0;

    public List<BookItemDto> Books { get; set; } = new();
}