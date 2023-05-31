using LibraryApi.Entites;

namespace LibraryApi.DTOs.User
{
    public record struct ChangePasswordDTO
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
