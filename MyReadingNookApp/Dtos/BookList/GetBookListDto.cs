namespace MyReadingNookApp.Dtos.BookList;

public class GetBookListDto
{
    public int Id { get; set; }
    
    public string ListName { get; set; }

    public DateOnly CreatedAt { get; set; }

    public List<string> BookCovers { get; set; } = new List<string>();
}

public class RemoveBookDto
{
    public int BookId { get; set; }
}