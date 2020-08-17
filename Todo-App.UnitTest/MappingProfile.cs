using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Business.Models;
using Todo_App.DAL.EfDbContext;
using Todo_App.Domain.Entities;

namespace Todo_App.UnitTest
{
    public class MyMappingProfile : Profile
    {
        public MyMappingProfile()
        {
            CreateMap<Todo, TodoDto>();
            CreateMap<TodoDto, Todo>();
        }
    }
}
