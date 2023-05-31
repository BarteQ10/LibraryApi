using Microsoft.AspNetCore.Mvc;
using LibraryApi.DTOs.User;
using LibraryApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using ServiceStack.Host;
using System.Security.Claims;
using NuGet.Protocol;
using LibraryApi.Entites;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            await _accountService.Register(dto);
            return Ok();

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var token = await _accountService.Login(dto);
            return Ok(token);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var token = await _accountService.Refresh(Int32.Parse(userId));
            return Ok(token);
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _accountService.ChangePassword(dto, Int32.Parse(userId));
            return Ok();
        }
    }
}
