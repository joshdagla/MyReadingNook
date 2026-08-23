namespace MyReadingNookApp.Dtos.BookList;

public class BookListFormat
{
    public int Id { get; set; }
    
    public string ListName { get; set; }

    public DateOnly CreatedAt { get; set; }

    public string? BookCovers { get; set; }
}