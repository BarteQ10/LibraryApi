using Microsoft.EntityFrameworkCore;
using LibraryApi.Data;
using LibraryApi.DTOs.Loan;
using LibraryApi.Entites;
using LibraryApi.DTOs.User;
using Azure.Core;

namespace LibraryApi.Services.Interfaces
{
    public class LoanResult
    {
        public Loan Loan { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
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

        public async Task<GetLoanDTO> GetLoanById(int loanId)
        {
            if (_context.Loans == null)
            {
                return null;
            }

            var loan = await _context.Loans
                .Include(b => b.Book)
                .Include(u => u.User)
                .FirstOrDefaultAsync(l => l.Id == loanId);
            if(loan == null)
            {
                return null;
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

        public async Task<LoanResult> CreateLoan(CreateLoanDTO request)
        {
            var book = await _context.Books.FindAsync(request.BookId);
            if (book == null)
            {
                return new LoanResult { ErrorMessage = "Book not found" };
            }

            if (!book.IsAvailable)
            {
                return new LoanResult { ErrorMessage = "Book is not available" };
            }

            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
            {
                return new LoanResult { ErrorMessage = "User not found" };
            }

            var loan = new Loan { BorrowDate = request.BorrowDate, ReturnDate = null, IsReturned = false, Book = book, User = user };
            book.IsAvailable = false;
            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            return new LoanResult { Loan = loan };
        }

        public async Task<LoanResult> FinishLoan(FinishLoanDTO request, int userId)
        {
            if (_context.Loans == null)
            {
                return new LoanResult { ErrorMessage = "Problem with Data Base" };
            }

            var loan = await _context.Loans
                .Include(b => b.Book)
                .Include(u => u.User)
                .FirstOrDefaultAsync(i => i.Id == request.LoanId);

            if (loan == null)
            {
                return new LoanResult { ErrorMessage = "Loan not found" };
            }

            if (loan.User.Id != userId)
            {
                return new LoanResult { ErrorCode = "401", ErrorMessage = "Loan not found" };
            }

            loan.Book.IsAvailable = true;
            loan.ReturnDate = request.ReturnDate;
            loan.IsReturned = true;
            await _context.SaveChangesAsync();

            return new LoanResult { Loan = loan };
        }

        public async Task<LoanResult> DeleteLoan(int loanId, int userId)
        {

            var loan = await _context.Loans
                .Include(b => b.Book)
                .Include(u => u.User)
                .FirstOrDefaultAsync(i => i.Id == loanId);
            if (loan == null)
            {
                return new LoanResult { ErrorMessage = "Loan not found" };
            }

            if(loan.User.Id != userId)
            {
                return new LoanResult { ErrorCode= "401", ErrorMessage = "Loan not found" };
            }

            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();

            return new LoanResult { };
        }
    }
}
