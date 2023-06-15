using LibraryApi.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Services.Interfaces
{
    public interface IAccountService
    {
        Task<bool> Register(RegisterDTO dto);
        Task<bool> ChangePassword(ChangePasswordDTO dto, int userId);
        Task<string> Login(LoginDTO dto);
        Task<string> Refresh(int userId);
        Task<IEnumerable<GetUserDTO>> GetUsers();
        Task<bool> SetAccountStatus(int id, bool isActive);
    }
}