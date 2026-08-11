using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyReadingNookApp.Dtos.Books;

public class BookAddDto
{
    [MaxLength(100)]
    public string Title { get; set; }

    [MaxLength(75)]
    public string Author { get; set; }

    [Unicode(false)]
    [MaxLength(13)]
    public string Isbn { get; set; }

    [MaxLength(30)]
    public string? Publisher { get; set; }

    [Column(TypeName = "TEXT")]
    public string? Description { get; set; }

    [Unicode(false)] 
    [MaxLength(300)]
    public string? ImagePath { get; set; }
}