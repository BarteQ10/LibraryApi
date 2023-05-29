namespace LibraryApi.DTOs.Book
{
    public record CreateBookDTO
    {
        public string Title { get; init; }
        public string Author { get; init; }
        public string Genre { get; init; }
        public string Description { get; init; }
        public string CoverImage { get; init; }
        public bool IsAvailable { get; init; }

        public CreateBookDTO() { }
    }
}
