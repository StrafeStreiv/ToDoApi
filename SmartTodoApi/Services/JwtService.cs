using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SmartTodoApi.Models;

namespace SmartTodoApi.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }

    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            // Создаем claims (утверждения) для токена
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.DisplayName)
            };

            // Получаем секретный ключ из конфигурации
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            // Создаем credentials (учетные данные) для подписи токена
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Создаем JWT токен
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryInMinutes"])),
                signingCredentials: creds);

            // Возвращаем токен в виде строки
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}