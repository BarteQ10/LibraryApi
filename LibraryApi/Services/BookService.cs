using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryApi.Data;
using LibraryApi.Entites;
using LibraryApi.DTOs.Book;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;

        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await _context.Books.OrderBy(c => c.Id).ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<bool> UpdateBookAsync(int id, CreateBookDTO request)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return false;
            }

            book.Author = request.Author;
            book.CoverImage = request.CoverImage;
            book.Description = request.Description;
            book.Genre = request.Genre;
            book.IsAvailable = request.IsAvailable;
            book.Title = request.Title;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Book> AddBookAsync(CreateBookDTO request)
        {
            var book = new Book
            {
                Author = request.Author,
                CoverImage = request.CoverImage,
                Description = request.Description,
                Genre = request.Genre,
                IsAvailable = request.IsAvailable,
                Title = request.Title
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return book;
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return false;
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
