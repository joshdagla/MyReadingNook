using System.ComponentModel.DataAnnotations;

namespace MyReadingNookApp.Dtos.Books;

// BookInfoDto is used for a PUT request through which users can add their personal notes for books
// and update a books status (want to read or already read)
public class BookInfoDto
{
    [MaxLength(250)]
    public string UserNote { get; set; }

    public string BookStatus { get; set; }
}