using Microsoft.EntityFrameworkCore;
using LibraryApi.DTOs.Loan;
using LibraryApi.Entites;
using LibraryApi.DTOs.User;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using LibraryApi.Data;
using LibraryApi.Services.Interfaces;
using System.Net.Mail;
using ServiceStack.Host;

namespace LibraryApi.Services
{
    public class LoanService : ILoanService
    {
        private readonly ApplicationDbContext _context;

        public LoanService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetLoanDTO>> GetLoansByUserId(int userId)
        {
            var loans = await _context.Loans
                .Include(b => b.Book)
                .Include(u => u.User)
                .Where(u => u.User.Id == userId)
                .OrderBy(l => l.Id)
                .ToListAsync();

            if (loans == null)
            {
                return null;
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

        public async Task<GetLoanDTO> GetLoanById(int loanId, int userId)
        {
            if (_context.Loans == null)
            {
                throw new Exception();
            }

            var loan = await _context.Loans
                .Include(b => b.Book)
                .Include(u => u.User)
                .FirstOrDefaultAsync(l => l.Id == loanId);
            if (loan == null)
            {
                throw new HttpException(404, "Loan not found");
            }
            if(loan.User.Id != userId) 
            {
                throw new UnauthorizedAccessException();
            }
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
            return dto;
        }

        public async Task<Loan> CreateLoan(CreateLoanDTO request)
        {
            var book = await _context.Books.FindAsync(request.BookId);
            if (book == null)
            {
                throw new HttpException(404, "Book not found");
            }

            if (!book.IsAvailable)
            {
                throw new HttpException(406, "Book is not available");
            }

            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
            {
                throw new HttpException(404, "User not found");
            }

            var loan = new Loan { BorrowDate = request.BorrowDate, ReturnDate = null, IsReturned = false, Book = book, User = user };
            book.IsAvailable = false;
            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            return loan;
        }

        public async Task<Loan> FinishLoan(FinishLoanDTO request, int userId)
        {
            if (_context.Loans == null)
            {
                throw new Exception();
            }

            var loan = await _context.Loans
                .Include(b => b.Book)
                .Include(u => u.User)
                .FirstOrDefaultAsync(i => i.Id == request.LoanId);

            if (loan == null)
            {
                throw new HttpException(404, "Loan not found");
            }

            if (loan.User.Id != userId)
            {
                throw new UnauthorizedAccessException();
            }

            loan.Book.IsAvailable = true;
            loan.ReturnDate = request.ReturnDate;
            loan.IsReturned = true;
            await _context.SaveChangesAsync();

            return loan;
        }

        public async Task<bool> DeleteLoan(int loanId, int userId)
        {

            var loan = await _context.Loans
                .Include(b => b.Book)
                .Include(u => u.User)
                .FirstOrDefaultAsync(i => i.Id == loanId);
            if (loan == null)
            {
                return false;
            }

            if (loan.User.Id != userId)
            {
                throw new UnauthorizedAccessException();
            }

            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
