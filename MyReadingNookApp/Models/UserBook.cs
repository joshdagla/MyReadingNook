using Microsoft.EntityFrameworkCore;

namespace MyReadingNookApp.Models
{
    [Keyless]
    public class UserBook
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTimeOffset AddedAt { get; set; } = DateTimeOffset.UtcNow;

        // Navigation properties
        public required User User { get; set; } = null;
        public required Book Book { get; set; } = null;
    }
}
