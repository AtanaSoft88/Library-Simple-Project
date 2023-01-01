using Library.Data;
using Library.Data.Models;
using Library.Models;
using Library.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
    public class BookService : IBookService
    {
        private readonly LibraryDbContext context;

        public BookService(LibraryDbContext context)
        {
            this.context = context;
        }        

        public async Task<IEnumerable<BookViewModel>> GetAllBooksCollection()
        {
            var booksCollection = await context.Books.Select(x=> new BookViewModel 
            {
                Id = x.Id,
                Title = x.Title,
                Author=x.Author,
                Category = x.Category.Name,
                ImageUrl = x.ImageUrl,
                Rating = x.Rating,
                Description = x.Description,
            }).ToListAsync();
            return booksCollection;
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await context.Categories.ToListAsync();
        }

        public async Task CurrentBookAdd(AddBookViewModel model)
        {
            Book bookToAdd = new Book()
            {
                Title = model.Title,
                Author = model.Author,               
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Rating = model.Rating,
                CategoryId = model.CategoryId,
            };
            await context.Books.AddAsync(bookToAdd);
            await context.SaveChangesAsync();
        }

        public async Task AddBookToCollection(int bookId, string userId)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new ArgumentException("Invalid User Id!");
            }
            var movie = await context.Books.FirstOrDefaultAsync(x => x.Id == bookId);
            if (movie == null)
            {
                throw new ArgumentException("Invalid Movie Id!");
            }           
            var isAlreadyAdd = user.ApplicationUsersBooks.Any(x => x.BookId == bookId);
            if (isAlreadyAdd)
            {
                return;
            }
            user.ApplicationUsersBooks.Add(new ApplicationUserBook()
            {
                BookId = bookId,
                ApplicationUserId = userId
            });
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BookViewModel>> GetReadBooks(string userId)
        {
            var currentUser = await context.Users
                                      .Include(x => x.ApplicationUsersBooks)
                                      .ThenInclude(x => x.Book)
                                      .ThenInclude(x => x.Category)
                                      .Where(u => u.Id == userId)
                                      .FirstOrDefaultAsync();

            var allReadBooks = currentUser.ApplicationUsersBooks.Select(x => new BookViewModel
            {
                Id = x.Book.Id,                
                Rating = x.Book.Rating,
                Author = x.Book.Author,
                ImageUrl = x.Book.ImageUrl,
                Title = x.Book.Title,
                Description = x.Book.Description,
                Category=x.Book.Category.Name,
            });
            return allReadBooks;
        }

        public async Task RemoveBookFromCollection(int bookId, string userId)
        {
            var currentUser = await context.Users.Include(x => x.ApplicationUsersBooks).ThenInclude(y => y.Book).ThenInclude(x => x.Category).FirstOrDefaultAsync(x => x.Id == userId);
            if (currentUser == null)
            {
                throw new ArgumentException("Invalid User Id!");
            }

            var currentBook = currentUser.ApplicationUsersBooks.FirstOrDefault(x => x.BookId == bookId);
            if (currentBook == null)
            {
                throw new ArgumentException("Invalid Book Id!");
            }

            currentUser.ApplicationUsersBooks.Remove(currentBook);

            await context.SaveChangesAsync();
        }

        public async Task EditBookService(EditViewModel model)
        {
            var book = await FindBookById(model.BookId);            
            
            if (book != null) 
            {
                book.Title = model.Title;                
                book.CategoryId=model.CategoryId;
                book.Description = model.Description;
                book.Author = model.Author;
                book.Rating = model.Rating;
                book.ImageUrl = model.ImageUrl;                
                await context.SaveChangesAsync();

            }
           
        }

        public async Task<Book> FindBookById(int? bookId)
        {
            return await context.Books.FindAsync(bookId);
        }
    }
}
