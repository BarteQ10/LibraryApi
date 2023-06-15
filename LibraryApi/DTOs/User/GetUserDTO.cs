using LibraryApi.Entites;

namespace LibraryApi.DTOs.User
{
    public record struct GetUserDTO
    {
        public int Id { get; init; }
        public string Email { get; init; }
        public bool IsActive { get; init; }
        public Role Role { get; init; }

    }
}
