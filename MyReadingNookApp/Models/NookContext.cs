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
        
        public DbSet<FriendsModel> Friends { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<UserBookModel>()
                .HasKey(ub => new {ub.UserId,ub.BookId });
            
            modelBuilder.Entity<UserBookModel>()
                .HasOne(ub => ub.UserModel)
                .WithMany()
                .HasForeignKey(ub => ub.UserId);
            
            modelBuilder.Entity<UserBookModel>()
                .HasOne(ub => ub.BookModel)
                .WithMany()
                .HasForeignKey(ub => ub.BookId);
            
            modelBuilder.Entity<FriendsModel>()
                .HasOne(f => f.Requester)
                .WithMany()
                .HasForeignKey(f => f.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FriendsModel>()
                .HasOne(f => f.Addressee)
                .WithMany()
                .HasForeignKey(f => f.AddresseeId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<FriendsModel>()
                .HasIndex(f => new { f.RequesterId, f.AddresseeId })
                .IsUnique();
        }
        
        public DbSet<BookListModel> BookLists { get; set; }
    }
}
