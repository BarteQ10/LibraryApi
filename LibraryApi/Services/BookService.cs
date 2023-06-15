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
using ServiceStack.Auth;
using static System.Reflection.Metadata.BlobBuilder;

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
        public async Task<IEnumerable<Book>> GetBooksAsync(string title, string author, string genre, string available, string sort)
        {
            IQueryable<Book> query = _context.Books
            .Where(b => b.Title.Contains(title) && b.Author.Contains(author) && b.Genre.Contains(genre));

            if (string.IsNullOrEmpty(sort))
            {
                query = query.OrderBy(c => c.Title);
            }
            else
            {
                query = query.OrderByDescending(c => c.Title);
            }
            if (!string.IsNullOrEmpty(available))
            {
                query = query.Where(b => b.IsAvailable.Equals(true));
            }
            List<Book> books = await query.ToListAsync();
            return books;
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

        public async Task<bool> UpdateBookAsync(int id, CreateBookDTO request, HttpContext httpContext)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                throw new HttpException(404, "Book not found");
            }
            try
            {
                var files = httpContext.Request.Form.Files;
                if (files != null && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        if (!book.CoverImage.Contains(file.FileName))
                        {
                            FileInfo fi = new FileInfo(file.FileName);
                            var newFileName = "Image" + DateTime.Now.ToBinary() + fi.Extension;
                            var path = Path.Combine(_hostingEnvironment.ContentRootPath, "Images", newFileName);
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                            if (book.CoverImage != "PlaceHolder.jpg")
                            {
                                FileInfo file1 = new FileInfo(Path.Combine(_hostingEnvironment.ContentRootPath, "Images", book.CoverImage));
                                if (file1.Exists)
                                {
                                    file1.Delete();
                                }
                            }
                            book.CoverImage = newFileName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new HttpException(500, ex.Message);
            }
            book.Author = request.Author;
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
                CoverImage = "PlaceHolder.jpg"
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
                        var path = Path.Combine(_hostingEnvironment.ContentRootPath, "Images" , newFileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        book.CoverImage = newFileName;
                    }
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
            FileInfo file1 = new FileInfo(Path.Combine(_hostingEnvironment.ContentRootPath + "Images" + book.CoverImage));
            if (file1.Exists && book.CoverImage != "PlaceHolder.jpg")
            {
                file1.Delete();
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return true;
        }       
    }
}
