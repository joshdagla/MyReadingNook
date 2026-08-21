namespace MyReadingNookApp.Dtos.Books;

public class BookDetailsResponseDto
{
    public int? BookId { get; set; }
    public string? Title { get; set; }

    public string? Author { get; set; }
    
    public string? Isbn { get; set; }

    public string? Publisher { get; set; }

    public string? Description { get; set; }

    public string? Thumbnail { get; set; }

    public bool IsInCollection { get; set; }

    public UserBookInfoDto? UserBookInfo { get; set; }
}

public class UserBookInfoDto
{
    public string? BookStatus { get; set; }

    public int? BookRating { get; set; }

    public string? UserNotes { get; set; }
}