using App_Core.Dtos;
using App_Core.Models;
using AutoMapper;

namespace App_Core.AutoMapper.Todo
{
    public class TodoItemMappingProfile:Profile
    {
        public TodoItemMappingProfile()
        {
            CreateMap<TodoItemPostPutDto, TodoItem>();
            CreateMap<TodoItem, TodoItemGetDto>()
                .ForMember(dest=>dest.CreatedDate, opt=>opt.MapFrom(source=>source.CreatedDate.ToString("MM/dd/yyyy HH:mm:ss")))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(source => source.ModifiedDate.ToString("MM/dd/yyyy HH:mm:ss")));
        }
    }
}
