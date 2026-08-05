using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace MyReadingNookApp.Models;

[Index(nameof(Isbn), IsUnique = true)]
public class BookModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [Required]
    [MaxLength(75)]
    public string Author { get; set; }

    [Required]
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
