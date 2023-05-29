using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryApi.Data;
using LibraryApi.Entites;
using LibraryApi.DTOs.Book;
using Microsoft.EntityFrameworkCore;
using LibraryApi.Services.Interfaces;
using ServiceStack.Host;
using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting.Internal;
using ServiceStack;
using Azure.Core;

namespace LibraryApi.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;
        public Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        public BookService(ApplicationDbContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await _context.Books.OrderBy(c => c.Id).ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                throw new HttpException(404, "Book not found");
            }
            else
            {
                return book;
            }
            
        }

        public async Task<bool> UpdateBookAsync(int id, CreateBookDTO request)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                throw new HttpException(404, "Book not found");
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

        public async Task<Book> AddBookAsync(CreateBookDTO request, HttpContext httpContext)
        {
            var book = new Book
            {
                Author = request.Author,
                Description = request.Description,
                Genre = request.Genre,
                IsAvailable = request.IsAvailable,
                Title = request.Title,
                CoverImage = request.CoverImage,
            };
            
            try
            {
                var files = httpContext.Request.Form.Files;
                if (files != null && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        FileInfo fi = new FileInfo(file.FileName);
                        var newFileName = "Image" + DateTime.Now.ToBinary() + fi.Extension;
                        var path = Path.Combine("", _hostingEnvironment.ContentRootPath + "\\Images\\" + newFileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        book.CoverImage = newFileName;
                    }
                }
                else
                {
                    book.CoverImage = "PlaceHolder.jpg";
                }
                
            }
            catch (Exception ex)
            {
                throw new HttpException(500, ex.Message);
            }
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return book;
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                throw new HttpException(404, "Book not found");
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
