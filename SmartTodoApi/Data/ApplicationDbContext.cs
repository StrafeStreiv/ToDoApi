using Microsoft.EntityFrameworkCore;
using SmartTodoApi.Models;

namespace SmartTodoApi.Data
{
    /// <summary>
    /// Контекст базы данных - основной класс для работы с Entity Framework Core
    /// Наследуется от DbContext - базового класса EF Core
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        // Конструктор, принимающий options (настройки подключения к БД)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSet представляют таблицы в базе данных
        public DbSet<User> Users { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }

        /// <summary>
        /// Метод для настройки моделей и связей между ними
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка пользователя
            modelBuilder.Entity<User>(entity =>
            {
                // Уникальный индекс для email
                entity.HasIndex(u => u.Email).IsUnique();

                // Ограничение длины
                entity.Property(u => u.Email).HasMaxLength(100);
                entity.Property(u => u.DisplayName).HasMaxLength(50);

                // Значение по умолчанию для CreatedAt
                entity.Property(u => u.CreatedAt).HasDefaultValueSql("NOW()");
            });

            // Настройка задачи
            modelBuilder.Entity<TodoItem>(entity =>
            {
                // Ограничение длины
                entity.Property(t => t.Title).HasMaxLength(200);

                // Внешний ключ и связь с пользователем
                entity.HasOne(t => t.User)
                      .WithMany(u => u.Todos)
                      .HasForeignKey(t => t.UserId)
                      .OnDelete(DeleteBehavior.Cascade); // При удалении пользователя удаляются его задачи

                // Значения по умолчанию
                entity.Property(t => t.CreatedAt).HasDefaultValueSql("NOW()");
                entity.Property(t => t.IsCompleted).HasDefaultValue(false);
            });
        }
    }
}