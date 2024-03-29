﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryApi.Data;
using LibraryApi.Entites;
using LibraryApi.DTOs.Book;
using LibraryApi.Services.Interfaces;
using LibraryApi.Services;
using Microsoft.AspNetCore.Authorization;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks(string title="", string author = "", string genre = "", string available = "" , string sort = "")
        {
            var books = await _bookService.GetBooksAsync(title, author, genre, available, sort);
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            return Ok(book);
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> PutBook(int id, [FromForm] CreateBookDTO request)
        {
            var httpContext = HttpContext;
            await _bookService.UpdateBookAsync(id, request, httpContext);
            return Ok("Updated");
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<ActionResult<Book>> PostBook([FromForm] CreateBookDTO request)
        {
            var httpContext = HttpContext;
            var book = await _bookService.AddBookAsync(request, httpContext);
            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            await _bookService.DeleteBookAsync(id);
            return NoContent();

        }
    }
}
