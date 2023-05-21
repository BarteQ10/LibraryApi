using LibraryApi.DTOs.User;

namespace LibraryApi.Services.Interfaces
{
    public interface IAccountService
    {
        Task<bool> Register(RegisterDTO dto);
        Task<string> Login(LoginDTO dto);
    }
}