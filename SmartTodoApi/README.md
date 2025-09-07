#  Smart Todo API

Умное REST API для управления задачами с аутентификацией JWT и PostgreSQL.

##  Возможности

- JWT аутентификация
- CRUD операции для задач
- PostgreSQL база данных
- Entity Framework Core с миграциями
- Swagger документация
- AutoMapper для DTO
- Валидация данных

##  Технологии

- **ASP.NET Core 7**
- **Entity Framework Core 7**
- **PostgreSQL**
- **JWT Authentication**
- **AutoMapper**
- **BCrypt.Net**
- **Swagger/OpenAPI**

##  Быстрый старт

### Предварительные требования

- [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Git](https://git-scm.com/)

### Установка

1. Клонируйте репозиторий:
```bash
git clone https://github.com/ваш-username/SmartTodoApi.git
cd SmartTodoApi
```

2.Настройте базу данных в appsettings.json:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=smart_todo_db;Username=postgres;Password=your_password"
  }
}
```

3.Примените миграции:
```bash
dotnet ef database update
```

4.Запустите приложение:
```bash
dotnet run
```
5.Откройте Swagger UI: https://localhost:PORT/swagger

## Документация API

### Аутентификация
- **POST /api/auth/register** - Регистрация пользователя
- **POST /api/auth/login** - Вход пользователя

### Задачи (требуют аутентификации)
- **GET /api/todos** - Получить все задачи пользователя
- **GET /api/todos/{id}** - Получить задачу по ID
- **POST /api/todos** - Создать новую задачу
- **PUT /api/todos/{id}** - Обновить задачу
- **PATCH /api/todos/{id}/toggle** - Переключить статус выполнения
- **DELETE /api/todos/{id}** - Удалить задачу

## Структура проекта 

```text
SmartTodoApi/
├── Controllers/          # API контроллеры
├── Data/                # Контекст базы данных
├── Dtos/               # Data Transfer Objects
├── Models/             # Модели Entity Framework
├── Services/           # Сервисы (JWT и др.)
├── Helpers/            # Вспомогательные классы
├── Program.cs          # Точка входа
└── appsettings.json    # Конфигурация
```
## Настройка проекта
### JWT Configuration

В appsettings.json настройте JWT:
```json
{
  "Jwt": {
    "Key": "YourSuperSecretKeyHere_Minimum16Characters",
    "Issuer": "SmartTodoApi",
    "Audience": "SmartTodoApiUsers",
    "ExpiryInMinutes": 60
  }
}
```

## Базы данных
Миграции применяются автоматически при запуске программы

## Лицензия
Этот проект распространяется под MIT License.

## Автор
StrafeStreiv