using Microsoft.EntityFrameworkCore;

namespace MyReadingNookApp.Models
{
    public class NookContext : DbContext
    {
        public NookContext(DbContextOptions<NookContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; } = null!;

        public DbSet<Book> Books { get; set; } = null!;
        
        public DbSet<UserBook> UserBooks { get; set; } = null!;
    }
}
