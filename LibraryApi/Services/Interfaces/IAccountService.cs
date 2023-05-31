using LibraryApi.DTOs.User;

namespace LibraryApi.Services.Interfaces
{
    public interface IAccountService
    {
        Task<bool> Register(RegisterDTO dto);
        Task<bool> ChangePassword(ChangePasswordDTO dto, int userId);
        Task<string> Login(LoginDTO dto);
        Task<string> Refresh(int userId);
    }
}