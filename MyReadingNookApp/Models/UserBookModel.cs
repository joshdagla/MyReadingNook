using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MyReadingNookApp.Models
{
    [Keyless]
    public class UserBookModel
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTimeOffset AddedAt { get; set; } = DateTimeOffset.UtcNow;
        
        [MaxLength(250)]
        public string? UserNotes { get; set; }

        // Navigation properties
        public required UserModel UserModel { get; set; } = null;
        public required BookModel BookModel { get; set; } = null;
    }
}
