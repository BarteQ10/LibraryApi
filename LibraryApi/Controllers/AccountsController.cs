using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using LibraryApi.Entites;
using LibraryApi.Data;

using Microsoft.AspNet.Identity;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using LibraryApi.DTOs.User;

namespace LibraryApi.Controllers
{
    [ApiController]
        [Route("api/[controller]")]
        public class AccountController : ControllerBase
        {
            private readonly ApplicationDbContext _context;
            private readonly IPasswordHasher<User> _passwordHasher;
            private readonly AuthenticationSettings _authenticationSettings;

            public AccountController(ApplicationDbContext context, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
            {
                _context = context;
                _passwordHasher = passwordHasher;
                _authenticationSettings = authenticationSettings;
            }
            [HttpPost("register")]
            public async Task<IActionResult> Register(RegisterDTO dto)
            {
            var newUser = new User()
            {
                Email = dto.Email,
                IsActive = false,
                Username = dto.Username,
                Loans=null
                };
                var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
                newUser.PasswordHash = hashedPassword;
                _context.Users.Add(newUser);
                _context.SaveChanges();
                return Ok();
            }
            [HttpPost("login")]
            public async Task<IActionResult> Login(LoginDTO dto)
            {
                var user = _context.Users
                .FirstOrDefault(u => u.Email == dto.Email);
                if (user is null)
                {
                    return BadRequest("Invalid username or password");
                }
                var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
                if (result == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Failed)
                {
                    return BadRequest("Invalid username or password");
                }
                var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),

            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);
                var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                    _authenticationSettings.JwtIssuer,
                    claims,
                    expires: expires,
                    signingCredentials: cred);
                var tokenHandler = new JwtSecurityTokenHandler();
                return Ok(tokenHandler.WriteToken(token));
            }


        }
    }
