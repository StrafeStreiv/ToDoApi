using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SmartTodoApi.Models
{
    /// <summary>
    /// Модель пользователя для базы данных
    /// </summary>
    public class User
    {
        public int Id { get; set; }

        // Атрибут [Required] означает, что поле обязательно для заполнения
        [Required]
        [EmailAddress] // Валидация формата email
        public string Email { get; set; } = string.Empty;

        // JsonIgnore - не возвращать это поле в API ответах
        [JsonIgnore]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string DisplayName { get; set; } = string.Empty;

        // Навигационное свойство - связь один-ко-многим с TodoItem
        // Virtual для lazy loading
        public virtual List<TodoItem> Todos { get; set; } = new List<TodoItem>();

        // Дата создания пользователя (устанавливается автоматически)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}