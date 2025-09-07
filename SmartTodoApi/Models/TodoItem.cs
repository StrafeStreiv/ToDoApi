using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTodoApi.Models
{
    /// <summary>
    /// Модель задачи для базы данных
    /// </summary>
    public class TodoItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;

        public bool IsCompleted { get; set; } = false;

        // Nullable DateTime? - значит поле не обязательное
        public DateTime? DueDate { get; set; }

        // Внешний ключ для связи с пользователем
        public int UserId { get; set; }

        // Навигационное свойство к пользователю
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}