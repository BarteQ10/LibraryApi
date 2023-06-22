using Microsoft.AspNetCore.Mvc;
using LibraryApi.DTOs.User;
using LibraryApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using ServiceStack.Host;
using System.Security.Claims;
using NuGet.Protocol;
using LibraryApi.Entites;
using Microsoft.AspNetCore.Authorization;
using NuGet.Common;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public class RefreshDTO
        {
            public int UserId { get; set; }
            public string RefreshToken { get; set; }
        }
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
        public async Task<IActionResult> Refresh(RefreshDTO dto)
        {
            var result = await _accountService.Refresh(dto.UserId, dto.RefreshToken);
            return Ok(result);
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _accountService.ChangePassword(dto, Int32.Parse(userId));
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("set-account-status/{id}")]
        public async Task<IActionResult> SetAccountStatus(int id, [FromBody] bool isActive)
        {
            await _accountService.SetAccountStatus(id, isActive);
            return Ok("Set up status "+isActive+" for user with id: "+id);

        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all-users")]
        public async Task<ActionResult<IEnumerable<GetUserDTO>>> GetUsers()
        {
            var users = await _accountService.GetUsers();
            return Ok(users);
        }
    }
}
