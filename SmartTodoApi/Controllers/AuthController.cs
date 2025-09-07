using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartTodoApi.Data;
using SmartTodoApi.Dtos;
using SmartTodoApi.Models;
using SmartTodoApi.Services;

namespace SmartTodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IJwtService jwtService, IConfiguration configuration)
        {
            _context = context;
            _jwtService = jwtService;
            _configuration = configuration;
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// POST: api/auth/register
        /// </summary>
        [HttpPost("register")]
        public async Task<ActionResult<UserProfileDto>> Register(UserRegisterDto registerDto)
        {
            // Проверяем, существует ли пользователь с таким email
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                return BadRequest("Пользователь с таким email уже существует");
            }

            // Хэшируем пароль с помощью BCrypt
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            // Создаем нового пользователя
            var user = new User
            {
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                DisplayName = registerDto.DisplayName
            };

            // Добавляем пользователя в базу данных
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Возвращаем информацию о пользователе (без пароля)
            return Ok(new UserProfileDto
            {
                Id = user.Id,
                Email = user.Email,
                DisplayName = user.DisplayName,
                CreatedAt = user.CreatedAt
            });
        }

        /// <summary>
        /// Вход пользователя
        /// POST: api/auth/login
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(UserLoginDto loginDto)
        {
            // Ищем пользователя по email
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            // Проверяем, существует ли пользователь и верный ли пароль
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return Unauthorized("Неверный email или пароль");
            }

            // Генерируем JWT токен
            var token = _jwtService.GenerateToken(user);

            // Возвращаем токен и информацию о пользователе
            return Ok(new LoginResponseDto
            {
                Token = token,
                User = new UserProfileDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    CreatedAt = user.CreatedAt
                },
                Expiry = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryInMinutes"]))
            });
        }
    }
}