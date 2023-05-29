using LibraryApi.DTOs.Book;
using LibraryApi.Entites;

namespace LibraryApi.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetBooksAsync();
        Task<Book> GetBookByIdAsync(int id);
        Task<bool> UpdateBookAsync(int id, CreateBookDTO request);
        Task<Book> AddBookAsync(CreateBookDTO request, HttpContext httpContext);
        Task<bool> DeleteBookAsync(int id);
    }
}