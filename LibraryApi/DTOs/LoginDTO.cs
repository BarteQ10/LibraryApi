namespace LibraryApi.DTOs
{
    public record struct LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; init; }

    }
}
