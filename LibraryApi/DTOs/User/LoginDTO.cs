namespace LibraryApi.DTOs.User
{
    public record struct LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; init; }

    }
}
