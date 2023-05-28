﻿using System.Collections.Generic;
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
            book.CoverImageData = request.CoverImageData;
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
                Description = request.Description,
                Genre = request.Genre,
                IsAvailable = request.IsAvailable,
                Title = request.Title
            };

            if (request.CoverImageData != null)
            {
                book.CoverImageData = request.CoverImageData;
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
