using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.EntityFrameworkCore;

namespace MyReadingNookApp.Models;

[Index(nameof(ListName), IsUnique = true)]
public class BookListModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string ListName { get; set; }

    public List<BookModel>? BookItems { get; set; } = new();

    [Column(TypeName = "TEXT")]
    public string? Summary { get; set; }

    public DateOnly DateCreated { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    
    public UserModel UserModel { get; set; } = null!;
}