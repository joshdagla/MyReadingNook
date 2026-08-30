namespace MyReadingNookApp.Dtos.UserBooks;

public class BookStatsResponseDto
{
    public int TotalBooks { get; set; } = 0;
    
    public int PendingBooks { get; set; } = 0;
    
    public int CompletedBooks { get; set; } = 0;
}