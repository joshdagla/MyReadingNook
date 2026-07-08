using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyReadingNookApp.Models;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; set; }

    [Required]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Email is missing")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is missing")]
    [StringLength(15, MinimumLength = 8)]
    public string Password { get; set; }

    public int TotalBookCount { get; set; } = 0;

    public int CompletedBookCount { get; set; }

    public int PendingBookCount { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
