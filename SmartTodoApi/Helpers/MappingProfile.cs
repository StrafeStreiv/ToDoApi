using AutoMapper;
using SmartTodoApi.Dtos;
using SmartTodoApi.Models;

namespace SmartTodoApi.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User -> UserProfileDto
            CreateMap<User, UserProfileDto>();

            // TodoItem -> TodoItemReadDto
            CreateMap<TodoItem, TodoItemReadDto>();

            // TodoItemCreateDto -> TodoItem
            CreateMap<TodoItemCreateDto, TodoItem>();

            // Можно также добавить для обновления
            CreateMap<TodoItemCreateDto, TodoItem>().ReverseMap();
        }
    }
}
