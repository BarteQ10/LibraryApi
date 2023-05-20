namespace LibraryApi.DTOs.Loan
{
    public record struct FinishLoanDTO
    {
        public int LoanId { get; init; }
        public DateTime ReturnDate { get; init; }

    }
}
