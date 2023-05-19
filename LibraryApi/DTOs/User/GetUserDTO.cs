namespace LibraryApi.DTOs.User
{
    public record struct GetUserDTO
    {
        public string Username { get; init; }
        public string Email { get; init; }

    }
}
