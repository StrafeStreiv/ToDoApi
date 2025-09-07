using System.ComponentModel.DataAnnotations;

namespace SmartTodoApi.Dtos
{
    /// <summary>
    /// DTO для регистрации пользователя
    /// Отделен от модели для безопасности и валидации
    /// </summary>
    public class UserRegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string DisplayName { get; set; } = string.Empty;
    }
}