using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartTodoApi.Data;
using SmartTodoApi.Dtos;
using SmartTodoApi.Models;
using System.Security.Claims;
namespace SmartTodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Требует аутентификации для всех методов контроллера
    public class TodosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TodosController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Вспомогательный метод для получения ID текущего пользователя из JWT токена
        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        }

        /// <summary>
        /// Получить все задачи текущего пользователя
        /// GET: api/todos
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemReadDto>>> GetTodos()
        {
            var userId = GetCurrentUserId();

            var todos = await _context.TodoItems
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return Ok(_mapper.Map<List<TodoItemReadDto>>(todos));
        }

        /// <summary>
        /// Получить задачу по ID
        /// GET: api/todos/5
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemReadDto>> GetTodo(int id)
        {
            var userId = GetCurrentUserId();

            var todo = await _context.TodoItems
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (todo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TodoItemReadDto>(todo));
        }

        /// <summary>
        /// Создать новую задачу
        /// POST: api/todos
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TodoItemReadDto>> CreateTodo(TodoItemCreateDto createDto)
        {
            var userId = GetCurrentUserId();

            var todo = _mapper.Map<TodoItem>(createDto);
            todo.UserId = userId;

            _context.TodoItems.Add(todo);
            await _context.SaveChangesAsync();

            // Возвращаем созданную задачу
            return CreatedAtAction(nameof(GetTodo),
                new { id = todo.Id },
                _mapper.Map<TodoItemReadDto>(todo));
        }

        /// <summary>
        /// Обновить задачу
        /// PUT: api/todos/5
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(int id, TodoItemCreateDto updateDto)
        {
            var userId = GetCurrentUserId();

            var todo = await _context.TodoItems
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (todo == null)
            {
                return NotFound();
            }

            // Обновляем поля
            todo.Title = updateDto.Title;
            todo.DueDate = updateDto.DueDate;
            todo.UpdatedAt = DateTime.UtcNow;

            _context.TodoItems.Update(todo);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content
        }

        /// <summary>
        /// Отметить задачу как выполненную/не выполненную
        /// PATCH: api/todos/5/toggle
        /// </summary>
        [HttpPatch("{id}/toggle")]
        public async Task<ActionResult<TodoItemReadDto>> ToggleTodo(int id)
        {
            var userId = GetCurrentUserId();

            var todo = await _context.TodoItems
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (todo == null)
            {
                return NotFound();
            }

            todo.IsCompleted = !todo.IsCompleted;
            todo.UpdatedAt = DateTime.UtcNow;

            _context.TodoItems.Update(todo);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<TodoItemReadDto>(todo));
        }

        /// <summary>
        /// Удалить задачу
        /// DELETE: api/todos/5
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var userId = GetCurrentUserId();

            var todo = await _context.TodoItems
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (todo == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todo);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content
        }
    }
}