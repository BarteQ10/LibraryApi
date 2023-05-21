using Microsoft.AspNetCore.Mvc;
using LibraryApi.DTOs.User;
using LibraryApi.Services.Interfaces;

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
            var result = await _accountService.Register(dto);
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Registration failed");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var token = await _accountService.Login(dto);
            if (token != null)
            {
                return Ok(token);
            }
            else
            {
                return BadRequest("Invalid username or password");
            }
        }
    }
}
