using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyReadingNookApp.Models;

[Index(nameof(Username), nameof(Email), IsUnique = true)]
public class UserModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    
    public int TotalBookCount { get; set; } = 0;

    public int CompletedBookCount { get; set; } = 0;

    public int PendingBookCount { get; set; } = 0;

    [AllowedValues("Public", "Private", "FriendsOnly")]
    [MaxLength(30)]
    public string ProfileVisibility { get; set; } = "Public";
    
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
