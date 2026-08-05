namespace MyReadingNookApp.Dtos.UserBooks;

public class AllBooksResponseDto
{
    public int BookId { get; set; }

    public string Title { get; set; }

    public string Author { get; set; }

    public string ImagePath { get; set; }

    public string BookStatus { get; set; }

    public int? BookRating { get; set; }
}