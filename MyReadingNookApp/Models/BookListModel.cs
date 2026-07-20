using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

    public List<BookModel>? BookItems { get; set; }
}