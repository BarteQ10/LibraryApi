namespace LibraryApi.DTOs
{
    public record struct CreateLoanDTO
    {
        public int BookId { get; init; }
        public int UserId { get; init; }

    }
}
