using LibraryApi.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Services.Interfaces
{
    public interface IAccountService
    {
        Task<bool> Register(RegisterDTO dto);
        Task<bool> ChangePassword(ChangePasswordDTO dto, int userId);
        Task<AuthenticationResult> Login(LoginDTO dto);
        Task<AuthenticationResult> Refresh(int userId, string refreshToken);
        Task<IEnumerable<GetUserDTO>> GetUsers();
        Task<bool> SetAccountStatus(int id, bool isActive);
    }
}