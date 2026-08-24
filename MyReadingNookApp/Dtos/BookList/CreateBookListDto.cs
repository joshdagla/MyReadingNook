namespace MyReadingNookApp.Dtos.BookList;

public class CreateBookListDto
{
    public string Name { get; set; } = string.Empty;
    public string? Summary { get; set; } = string.Empty;
}