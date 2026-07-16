using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MyReadingNookApp.Models;

[Index(nameof(Username), nameof(Email), IsUnique = true)]
public class UserModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string PasswordHash { get; set; }
    
    // TODO: Need to alter book list functionality. Make a new model for book lists that has UserId for navigation
    public List<BookModel>? BookLists { get; set; }

    public int TotalBookCount { get; set; } = 0;

    public int CompletedBookCount { get; set; }

    public int PendingBookCount { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
