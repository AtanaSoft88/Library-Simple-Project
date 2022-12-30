using Library.Data.Models;
using Library.Models;

namespace Library.Services.Contracts
{
    public interface IBookService
    {
        Task<IEnumerable<BookViewModel>> GetAllBooksCollection();
        Task<IEnumerable<Category>> GetAllCategories();
        Task CurrentBookAdd(AddBookViewModel model);
        Task AddBookToCollection(int movieId, string userId);
        Task EditBookService(EditViewModel model);
        Task<IEnumerable<BookViewModel>> GetReadBooks(string userId);        
        Task RemoveBookFromCollection(int movieId, string userId);
        Task<Book> FindBookById(int? bookId);
    }
}
