using System.ComponentModel.DataAnnotations;

namespace SmartTodoApi.Dtos
{
    public class TodoItemCreateDto
    {
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;

        public DateTime? DueDate { get; set; }
    }
}