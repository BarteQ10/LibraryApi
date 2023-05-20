using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryApi.Data;
using LibraryApi.Entites;
using LibraryApi.DTOs.Loan;
using LibraryApi.DTOs.User;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LoansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Loans
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<GetLoanDTO>>> GetLoans(int id)
        {
            var loans = await _context.Loans
                .Include(b => b.Book)
                .Include(u => u.User)
                .Where(u => u.User.Id == id)
                .OrderBy(l => l.Id)
                .ToListAsync();

            if (loans == null)
            {
                return NotFound();
            }

            var loansDTO = new List<GetLoanDTO>();
            foreach (var loan in loans)
            {
                var userDTO = new GetUserDTO { Email = loan.User.Email, Username = loan.User.Username };

                var dto = new GetLoanDTO
                {
                    Id = loan.Id,
                    Book = loan.Book,
                    BorrowDate = loan.BorrowDate,
                    IsReturned = loan.IsReturned,
                    ReturnDate = loan.ReturnDate,
                    User = userDTO
                };
                loansDTO.Add(dto);
            }

            return loansDTO;
        }

        // GET: api/Loans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Loan>> GetLoan(int id)
        {
            if (_context.Loans == null)
            {
                return NotFound();
            }
            var loan = await _context.Loans.FindAsync(id);

            if (loan == null)
            {
                return NotFound();
            }

            return loan;
        }

        // POST: api/Loans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("create")]
        public async Task<ActionResult<Loan>> CreateLoan(CreateLoanDTO request)
        {
            if (_context.Loans == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Loans'  is null.");
            }
            var book = await _context.Books.FindAsync(request.BookId);
            if (book == null)
            {
                return NotFound("Book not Found");
            }

            if(book.IsAvailable == false)
            {
                return NotFound("Book not Available");
            }

            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
            {
                return NotFound("User not Found");
            }
            var loan = new Loan { BorrowDate = request.BorrowDate, ReturnDate = null, IsReturned = false, Book = book, User = user };
            book.IsAvailable = false;
            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLoan", new { id = loan.Id }, null);
        }

        [HttpPost("end")]
        public async Task<ActionResult<Loan>> EndLoan(FinishLoanDTO request)
        {
            if (_context.Loans == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Loans'  is null.");
            }
            var loan = await _context.Loans
                .Include(b => b.Book)
                .Include(u => u.User)
                .FirstOrDefaultAsync(i => i.Id == request.LoanId);

            if (loan == null)
            {
                return NotFound("Loan not Found");
            }

            loan.Book.IsAvailable = true;
            loan.ReturnDate = request.ReturnDate;
            loan.IsReturned = true;
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Loans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            if (_context.Loans == null)
            {
                return NotFound();
            }
            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
            {
                return NotFound();
            }

            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoanExists(int id)
        {
            return (_context.Loans?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
