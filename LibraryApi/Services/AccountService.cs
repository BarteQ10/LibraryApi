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
using LibraryApi.Services.Interfaces;
using ServiceStack.Host;
using PasswordVerificationResult = Microsoft.AspNetCore.Identity.PasswordVerificationResult;
using Microsoft.Identity.Client;

namespace LibraryApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(ApplicationDbContext context, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }

        public async Task<bool> Register(RegisterDTO dto)
        {
            var newUser = new User()
            {
                Email = dto.Email,
                IsActive = false,
                Loans = null,
                Role = dto.Role,
                RefreshToken = "",
                RefreshTokenExpires = DateTime.UtcNow,
            };
            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
            newUser.PasswordHash = hashedPassword;
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ChangePassword(ChangePasswordDTO dto, int UserId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == UserId);
            if (user is null)
            {
                throw new HttpException(404, "User not found");
            }
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.CurrentPassword);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new HttpException(400, "Invalid current password");
            }
            var newPasswordHash = _passwordHasher.HashPassword(user, dto.NewPassword);
            user.PasswordHash = newPasswordHash;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<AuthenticationResult> Login(LoginDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user is null)
            {
                throw new HttpException(404, "Invalid username or password");
            }
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new HttpException(404, "Invalid username or password");
            }
            user.RefreshToken = Guid.NewGuid().ToString();
            user.RefreshTokenExpires = DateTime.Now.AddDays(_authenticationSettings.RefreshTokenExpires).ToUniversalTime();
            await _context.SaveChangesAsync();
            return new AuthenticationResult
            {
                JwtToken = createToken(user),
                RefreshToken = user.RefreshToken
            };
        }

        public async Task<AuthenticationResult> Refresh(int userId, string refreshToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
            {
                throw new HttpException(404, "Invalid username or password");
            }
            if (DateTime.Now > user.RefreshTokenExpires)
            {
                throw new HttpException(400, "Token expired");
            }
            if (user.RefreshToken != refreshToken)
            {
                throw new HttpException(400, "Token is not valid");
            }
            user.RefreshToken = Guid.NewGuid().ToString();
            user.RefreshTokenExpires = DateTime.Now.AddDays(_authenticationSettings.RefreshTokenExpires).ToUniversalTime();
            await _context.SaveChangesAsync();
            return new AuthenticationResult
            {
                JwtToken = createToken(user),
                RefreshToken = user.RefreshToken
            };

        }
        private string createToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, $"{user.Role}")
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
            return tokenHandler.WriteToken(token);
        }

        public async Task<IEnumerable<GetUserDTO>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            var userDtos = users.Select(user => new GetUserDTO
            {
                Id = user.Id,
                Email = user.Email,
                IsActive = user.IsActive,
                Role = user.Role
            });

            return userDtos;
        }

        public async Task<bool> SetAccountStatus(int id, bool isActive)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new HttpException(404, "User not found");
            }

            user.IsActive = isActive;
            await _context.SaveChangesAsync();
            return true;
        }
    }

    
}
