using HRM.Data;
using HRM.DTOs;
using HRM.Interfaces;
using HRM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HRM.Services
{
    public class AuthService : IAuthService
    {
        private const int PasswordIterations = 100000;
        private readonly HRMContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(HRMContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseVM?> Signup(SignupVM signupVM)
        {
            var email = signupVM.Email?.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(signupVM.FullName) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(signupVM.Password) || signupVM.Password.Length < 6)
            {
                return null;
            }

            var exists = await _context.appUsers.AnyAsync(x => x.StrEmail == email);
            if (exists)
            {
                return null;
            }

            CreatePasswordHash(signupVM.Password, out var hash, out var salt);
            var user = new AppUser
            {
                StrFullName = signupVM.FullName.Trim(),
                StrEmail = email,
                PasswordHash = hash,
                PasswordSalt = salt,
                DteCreatedAt = DateTime.Now
            };

            await _context.appUsers.AddAsync(user);
            await _context.SaveChangesAsync();

            return BuildResponse(user);
        }

        public async Task<AuthResponseVM?> Login(LoginVM loginVM)
        {
            var email = loginVM.Email?.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(loginVM.Password))
            {
                return null;
            }

            var user = await _context.appUsers.FirstOrDefaultAsync(x => x.StrEmail == email);
            if (user == null || VerifyPassword(loginVM.Password, user.PasswordHash, user.PasswordSalt) == false)
            {
                return null;
            }

            return BuildResponse(user);
        }

        private AuthResponseVM BuildResponse(AppUser user)
        {
            return new AuthResponseVM
            {
                UserId = user.IntUserId,
                FullName = user.StrFullName,
                Email = user.StrEmail,
                Token = CreateToken(user)
            };
        }

        private string CreateToken(AppUser user)
        {
            var jwt = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"] ?? throw new InvalidOperationException("JWT key is missing.")));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.IntUserId.ToString()),
                new Claim(ClaimTypes.Name, user.StrFullName),
                new Claim(ClaimTypes.Email, user.StrEmail)
            };

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(16);
            hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, PasswordIterations, HashAlgorithmName.SHA256, 32);
        }

        private static bool VerifyPassword(string password, byte[] expectedHash, byte[] salt)
        {
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, PasswordIterations, HashAlgorithmName.SHA256, 32);
            return CryptographicOperations.FixedTimeEquals(hash, expectedHash);
        }
    }
}
