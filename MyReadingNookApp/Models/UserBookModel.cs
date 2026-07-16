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

        [AllowedValues("Wish to Read", "Completed", ErrorMessage = "Invalid Book Status")]
        public string BookStatus { get; set; }

        // Rating given out of 5 stars
        [AllowedValues(1, 2, 3, 4, 5, ErrorMessage = "Invalid Book Rating")]
        public int BookRating { get; set; }

        // Navigation properties
        public required UserModel UserModel { get; set; } = null;
        public required BookModel BookModel { get; set; } = null;
    }
}
