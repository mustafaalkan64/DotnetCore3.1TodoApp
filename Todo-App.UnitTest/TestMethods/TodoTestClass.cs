using AutoMapper;
using MemoryCache.Testing.Moq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using Todo_App.Core.Consts;
using Todo_App.Business.Models;
using Todo_App.DAL.Abstract;
using Todo_App.DAL.EfDbContext;
using Todo_App.Domain.Entities;
using Todo_App.Business.Abstract;
using Todo_App.Business.Services;

namespace Todo_App.UnitTest.TestMethods
{
    [TestFixture]
    public class TodoListTest
    {
        private Mock<IGenericRepository<Todo>> _todoRepository;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IConfiguration> _configuration;
        private IMemoryCache _cache;
        private Mock<ILogger<TodoManager>> _logger;
        private IMapper _mapper;
        private List<Todo> todoList;
        private Todo todo;

        [SetUp]
        public void Setup()
        {
            _todoRepository = new Mock<IGenericRepository<Todo>>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _configuration = new Mock<IConfiguration>();
            _cache = Create.MockedMemoryCache();
            _logger = new Mock<ILogger<TodoManager>>();
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MyMappingProfile());
            });
            _mapper = mockMapper.CreateMapper();
            
            todoList.Add(new Todo()
            {
                Id = 1,
                Name = "Test Name 1",
                Desc = "Lorem Ipsum",
                CreateDate = DateTime.Now,
                CreatedBy = 1,
                UpdateDate = DateTime.Now,
                UpdatedBy = 2,
                CreatedByUser = new Users() { Id = 1, FirstName = "Mustafa", LastName = "Alkan", Email = "mustafaalkan64@gmail.com" },
                UpdatedByUser = new Users() { Id = 2, FirstName = "Cem", LastName = "Elma", Email = "cemelma@hotmail.com" }
            });
            todoList.Add(new Todo()
            {
                Id = 2,
                Name = "Test Name 2",
                Desc = "Lorem Ipsum",
                CreateDate = DateTime.Now,
                CreatedBy = 1,
                UpdateDate = DateTime.Now,
                UpdatedBy = 1,
                CreatedByUser = new Users() { Id = 1, FirstName = "Mustafa", LastName = "Alkan", Email = "mustafaalkan64@gmail.com" },
                UpdatedByUser = new Users() { Id = 1, FirstName = "Cem", LastName = "Elma", Email = "cemelma@hotmail.com" }
            });

            todoList.Add(new Todo()
            {
                Id = 3,
                Name = "Test Name 1",
                Desc = "Lorem Ipsum",
                CreateDate = DateTime.Now,
                CreatedBy = 2,
                UpdateDate = DateTime.Now,
                UpdatedBy = 1,
                CreatedByUser = new Users() { Id = 2, FirstName = "Mustafa", LastName = "Alkan", Email = "mustafaalkan64@gmail.com" },
                UpdatedByUser = new Users() { Id = 1, FirstName = "Cem", LastName = "Elma", Email = "cemelma@hotmail.com" }
            });

            todoList.Add(new Todo()
            {
                Id = 4,
                Name = "Test Name 1",
                Desc = "Lorem Ipsum",
                CreateDate = DateTime.Now,
                CreatedBy = 2,
                UpdateDate = DateTime.Now,
                UpdatedBy = 2,
                CreatedByUser = new Users() { Id = 2, FirstName = "Mustafa", LastName = "Alkan", Email = "mustafaalkan64@gmail.com" },
                UpdatedByUser = new Users() { Id = 2, FirstName = "Cem", LastName = "Elma", Email = "cemelma@hotmail.com" }
            });
            todo = todoList[0];
        }

        [Test]
        public void TodoListService_GetAll_Method_Should_Return_All_TodoList()
        {
            _unitOfWork.Setup(a => a.TodoRepository.GetAllAsync(It.IsAny<Expression<Func<Todo, object>>[]>()))
                .ReturnsAsync(new List<Todo>() { todoList[0] });

            var todoService = new TodoManager(_unitOfWork.Object, _configuration.Object, _cache, _logger.Object, _mapper);
            var testTodoList = todoService.Get().Result;

            Assert.IsTrue(testTodoList.ToList().Count > 0);
        }

        [Test]
        public void TodoListService_Given_TodoList_Id_Should_Get_TodoList_Name()
        {
           
            var todoRepositoryMock = new Mock<IGenericRepository<Todo>>();
            todoRepositoryMock.Setup(m => m.GetByIdAsync(todo.Id)).ReturnsAsync(todo).Verifiable();

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.TodoRepository).Returns(todoRepositoryMock.Object);

            ITodoService todoService = new TodoManager(unitOfWorkMock.Object, _configuration.Object, _cache, _logger.Object, _mapper);
            //Act
            var actual = todoService.Get(todo.Id).Result;

            ////Assert
            todoRepositoryMock.Verify();//verify that GetByID was called based on setup.
            Assert.IsNotNull(actual);//assert that a result was returned
            Assert.AreEqual(todo.Name, actual.Name);//assert that actual result was as expected
        }

        [Test]
        public void TodoListService_Should_Add_Successfuly()
        {
            var testObject = new Todo();
            var expected = new Todo();
            var todoRepositoryMock = new Mock<IGenericRepository<Todo>>();
            todoRepositoryMock.Setup(m => m.AddAsync(It.IsAny<Todo>())).Verifiable();

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.TodoRepository).Returns(todoRepositoryMock.Object);
            ITodoService todoService = new TodoManager(unitOfWorkMock.Object, _configuration.Object, _cache, _logger.Object, _mapper);
            
            //Act
            var actual = todoService.Add(new TodoDto() { Name = "Test", Desc = "Test Desc", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, CreatedBy = 1, UpdatedBy = 2 }).Result;

            ////Assert
            todoRepositoryMock.Verify();//verify that GetByID was called based on setup.
            Assert.AreEqual(actual, 0);//assert that actual result was as expected
        }

        [Test]
        public void TodoListService_Should_Updated_Successfuly()
        {
            var testObject = new Todo();
            var expected = new Todo();
            var todoId = 1;
            var todoRepositoryMock = new Mock<IGenericRepository<Todo>>();
            todoRepositoryMock.Setup(m => m.GetByIdAsync(todoId)).ReturnsAsync(todo).Verifiable();

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.TodoRepository).Returns(todoRepositoryMock.Object);

            ITodoService todoService = new TodoManager(unitOfWorkMock.Object, _configuration.Object, _cache, _logger.Object, _mapper);
            
            //Act
            var actual = todoService.Update(todoId, 
                    new TodoDto() 
                        {   Name = "Test Update", 
                            Desc = "Test Desc Update", 
                            UpdateDate = DateTime.Now, 
                            UpdatedBy = 2 
                        }, 
                    updatedUserId: 2).Result;

            ////Assert
            todoRepositoryMock.Verify();//verify that GetByID was called based on setup.
            Assert.AreEqual(actual.Response, HttpStatusCodeEnum.Ok.ToString());
            Assert.AreEqual(actual.Status, true);
        }

    }
}
