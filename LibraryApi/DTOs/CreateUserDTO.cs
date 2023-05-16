namespace LibraryApi.DTOs
{
    public record struct CreateUserDTO
    {
        public string Username { get; init; }
        public string Password { get; init; }
        public string Email { get; init; }

    }
}
