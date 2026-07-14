using Microsoft.EntityFrameworkCore;

namespace MyReadingNookApp.Models
{
    [Keyless]
    public class UserBookModel
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTimeOffset AddedAt { get; set; } = DateTimeOffset.UtcNow;

        // Navigation properties
        public required UserModel UserModel { get; set; } = null;
        public required BookModel BookModel { get; set; } = null;
    }
}
