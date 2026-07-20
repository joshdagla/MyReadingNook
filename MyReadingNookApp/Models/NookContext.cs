using Microsoft.EntityFrameworkCore;

namespace MyReadingNookApp.Models
{
    public class NookContext : DbContext
    {
        public NookContext(DbContextOptions<NookContext> options)
            : base(options) { }

        public DbSet<UserModel> Users { get; set; } = null!;

        public DbSet<BookModel> Books { get; set; } = null!;
        
        public DbSet<UserBookModel> UserBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<UserBookModel>()
                .HasKey(ub => new {ub.UserId,ub.BookId });
        }
        
        public DbSet<BookListModel> BookLists { get; set; }
    }
}
