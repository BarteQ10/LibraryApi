namespace LibraryApi.DTOs.User
{
    public record struct RegisterDTO
    {
        public string Username { get; init; }
        public string Email { get; set; }
        public string Password { get; init; }
        public string ConfirmPassword { get; set; }

    }
}
