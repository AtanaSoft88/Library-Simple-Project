using Library.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library.Data
{
    public class LibraryDbContext : IdentityDbContext<ApplicationUser>
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }
        public DbSet<Book> Books { get; set; }            
        public DbSet<Category> Categories { get; set; }
        public DbSet<ApplicationUserBook> ApplicationUserBooks { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUserBook>(e =>
            {
                e.HasKey(ub => new { ub.ApplicationUserId, ub.BookId }); // Composite PKs
            });
            builder.Entity<ApplicationUser>()
                .Property(u => u.UserName)
                .HasMaxLength(20)
                .IsRequired();
            builder.Entity<ApplicationUser>()
                .Property(p => p.Email)
                .HasMaxLength(60)
                .IsRequired();
            builder.Entity<Book>()
                .Property(r => r.Rating)
                .HasPrecision(18, 2)      
                .IsRequired();

            builder
               .Entity<Book>()
               .HasData(new Book()
               {
                   Id = 5,
                   Title = "Lorem Ipsum",
                   Author = "Dolor Sit",
                   ImageUrl = "https://img.freepik.com/free-photo/concept-exams-tests-space-text_185193-79222.jpg?w=1380&t=st=1666039091~exp=1666039691~hmac=f17f061a73cc0d6055208ea2945dccd9ce2112420a552e7e0e9ff1ccbd9b1d52",
                   Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                   CategoryId = 1,
                   Rating = 9.5m
               });

            builder
           .Entity<Category>()
           .HasData(new Category()
           {
               Id = 1,
               Name = "Action"
           },
           new Category()
           {
               Id = 2,
               Name = "Biography"
           },
           new Category()
           {
               Id = 3,
               Name = "Children"
           },
           new Category()
           {
               Id = 4,
               Name = "Crime"
           },
           new Category()
           {
               Id = 5,
               Name = "Fantasy"
           });


            base.OnModelCreating(builder);
        }
    }
}