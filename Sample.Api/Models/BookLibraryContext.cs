using Microsoft.EntityFrameworkCore;
using Sample.Api.Models.Entities;

namespace Sample.Api.Models
{
    public class BookLibraryContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        private string _conn = "";
        public BookLibraryContext(string conn)
        {
            _conn = conn;
        }

        public BookLibraryContext(DbContextOptions<BookLibraryContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_conn);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>().HasData(new Author
            {
                Id = 1,
                FirstName = "Victor Hugo",
                LastName = "Hogo"
            },
            new Author
            {
                Id = 2,
                FirstName = "Lev",
                LastName = "Tolstoy"
            });

            modelBuilder.Entity<Book>().HasData(new Book
            {
                Id = 1,
                AuthorId = 1,
                Name = "Notre Dame'ın Kamburu",
                ISBN = "AA-11-12"
            },
            new Book
            {
                Id = 2,
                AuthorId = 1,
                Name = "Sefiller",
                ISBN = "AA-11-13"
            },
             new Book
             {
                 Id = 3,
                 AuthorId = 2,
                 Name = "İnsan Ne İle Yaşar",
                 ISBN = "AA-11-14"
             },
              new Book
              {
                  Id = 4,
                  AuthorId = 2,
                  Name = "Savaş ve Barış",
                  ISBN = "BB-11-11"
              }

            );

            base.OnModelCreating(modelBuilder);
        }

    }
}
