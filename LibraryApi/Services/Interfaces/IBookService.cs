﻿using LibraryApi.DTOs.Book;
using LibraryApi.Entites;

namespace LibraryApi.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetBooksAsync(string title , string author , string genre , string available, string sort );
        Task<Book> GetBookByIdAsync(int id);
        Task<bool> UpdateBookAsync(int id, CreateBookDTO request, HttpContext httpContext);
        Task<Book> AddBookAsync(CreateBookDTO request, HttpContext httpContext);
        Task<bool> DeleteBookAsync(int id);
    }
}