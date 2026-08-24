using MyReadingNookApp.Dtos.UserBooks;

namespace MyReadingNookApp.Dtos.BookList;

public class CollectionDetailDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;

    public DateOnly CreatedAt { get; set; }

    public List<AllBooksResponseDto> Books { get; set; } = new List<AllBooksResponseDto>();
}