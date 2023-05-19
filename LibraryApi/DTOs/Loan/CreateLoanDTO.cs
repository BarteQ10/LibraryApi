using LibraryApi.Entites;

namespace LibraryApi.DTOs.Loan
{
    public record struct CreateLoanDTO
    {
        public int BookId { get; init; }
        public int UserId { get; init; }
        public DateTime BorrowDate { get; init; }
        public DateTime ReturnDate { get; init; }
        public bool IsReturned { get; init; }

    }
}
