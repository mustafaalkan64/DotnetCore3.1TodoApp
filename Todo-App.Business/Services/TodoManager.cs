using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Todo_App.Core.Consts;
using Todo_App.Business.Abstract;
using Todo_App.DAL.Abstract;
using Todo_App.Domain.Entities;
using Todo_App.Business.Models;

namespace Todo_App.Business.Services
{
    public class TodoManager : ITodoService
    {
        private readonly IUnitOfWork _uow;
        private IConfiguration _configuration;
        private IMemoryCache _cache;
        private readonly ILogger<TodoManager> _logger;
        private readonly IMapper _mapper;
        private Expression<Func<Todo, object>>[] _includes;
        private IEnumerable<Todo> todoCacheList;
        private IEnumerable<Todo> todoList;
        private IList<Todo> todoDataSet;
        private readonly string cacheKey = "todoListWithIncludes";

        public TodoManager(IUnitOfWork unit, 
            IConfiguration configuration, 
            IMemoryCache cache, 
            ILogger<TodoManager> logger, 
            IMapper mapper)
        {
            _uow = unit;
            _configuration = configuration;
            _cache = cache;
            _logger = logger;
            _mapper = mapper;
            _includes = new Expression<Func<Todo, object>>[]
            {
                (x => x.CreatedByUser),
                (x => x.UpdatedByUser)
            };
        }

        /// <summary>
        /// // Create Movie
        /// </summary>
        /// <param name="movie">Movie Model Parameter</param>
        /// <returns></returns>
        public async Task<int> Add(TodoDto todoDto)
        {
            try
            {
                var todo = _mapper.Map<Todo>(todoDto);
                await _uow.TodoRepository.AddAsync(todo);
                await _uow.Commit();

                _cache.TryGetValue(cacheKey, out todoCacheList);
                if (todoCacheList != null)
                {
                    var list = todoCacheList.ToList();
                    list.Add(todo);
                    _cache.Set(cacheKey, list.AsEnumerable());
                }

                _logger.LogInformation("Todo Created Successfuly", todo);
                return todo.Id;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Error During Add Todo", todoDto);
                throw e;
            }

        }


        public async Task<WebApiResponse> Update(int? todoId, TodoDto entity, int updatedUserId)
        {
            try
            {
                var todo = await _uow.TodoRepository.GetByIdAsync(todoId);
                if(todo!= null)
                {
                    todo = _mapper.Map<Todo>(entity);
                    todo.UpdateDate = DateTime.Now;
                    todo.UpdatedBy = updatedUserId;

                    await _uow.TodoRepository.UpdateAsync(todo);
                    await _uow.Commit();

                    _cache.TryGetValue(cacheKey, out todoCacheList);
                    if (todoCacheList != null)
                    {
                        var list = todoCacheList.ToList();
                        var item = list.SingleOrDefault(a => a.Id == todoId);
                        if(item != null)
                        {
                            item.Desc = todo.Desc;
                            item.Name = todo.Name;
                            item.UpdateDate = DateTime.Now;
                            item.UpdatedBy = todo.UpdatedBy;
                        }
                        _cache.Set(cacheKey, list.AsEnumerable());
                    }

                    return new WebApiResponse()
                    {
                        Response = HttpStatusCodeEnum.Ok.ToString(),
                        Status = true
                    };
                }
                else
                {
                    return new WebApiResponse()
                    {
                        Response = HttpStatusCodeEnum.NotFound.ToString(),
                        Status = false
                    };
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Error During Update ToDo", entity);
                throw e;
            }
        }

        public async Task<WebApiResponse> UpdateStatus(int? todoId, int status, int updatedUserId)
        {
            try
            {
                var todo = await _uow.TodoRepository.FindByAsync(a => a.Id == todoId);
                if (todo != null)
                {
                    todo.Status = status;
                    todo.UpdateDate = DateTime.Now;
                    todo.UpdatedBy = updatedUserId;

                    await _uow.TodoRepository.UpdateAsync(todo);
                    await _uow.Commit();

                    _cache.TryGetValue(cacheKey, out todoCacheList);
                    if (todoCacheList != null)
                    {
                        var list = todoCacheList.ToList();
                        var item = list.SingleOrDefault(a => a.Id == todoId);
                        if (item != null)
                        {
                            item.Status = status;
                        }
                        _cache.Set(cacheKey, list.AsEnumerable());
                    }

                    return new WebApiResponse()
                    {
                        Response = HttpStatusCodeEnum.Ok.ToString(),
                        Status = true
                    };
                }
                else
                {
                    return new WebApiResponse()
                    {
                        Response = HttpStatusCodeEnum.NotFound.ToString(),
                        Status = false
                    };
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Error During Update ToDo", todoId);
                throw e;
            }
        }

        public async Task<WebApiResponse> Delete(int? todoId)
        {
            try
            {
                var todo = await _uow.TodoRepository.FindByAsync(a => a.Id == todoId);
                if (todo != null)
                {
                    await _uow.TodoRepository.DeleteAsync(todo);
                    await _uow.Commit();

                    _cache.TryGetValue(cacheKey, out todoCacheList);
                    if (todoCacheList != null)
                    {
                        var list = todoCacheList.ToList();
                        var item = list.SingleOrDefault(a => a.Id == todoId);
                        if (item != null)
                        {
                            list.Remove(item);
                        }
                        _cache.Set(cacheKey, list.AsEnumerable());
                    }
                    return new WebApiResponse()
                    {
                        Response = HttpStatusCodeEnum.Ok.ToString(),
                        Status = true
                    };
                }
                else
                {
                    return new WebApiResponse()
                    {
                        Response = HttpStatusCodeEnum.NotFound.ToString(),
                        Status = false
                    };
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Error During Delete Todo", todoId);
                throw e;
            }
        }

        public async Task<IEnumerable<Todo>> Get()
        {
            try
            {
                _cache.TryGetValue(cacheKey, out todoCacheList);
                if (todoCacheList == null)
                {
                    todoList = await _uow.TodoRepository.GetAllAsync(_includes);
                    if(todoList != null)
                    {
                        _cache.Set(cacheKey, todoList);
                        todoDataSet = todoList.ToList();
                    }
                    else 
                       return null;
                }
                else
                {
                    todoDataSet = todoCacheList.ToList();
                }

                return todoDataSet.OrderByDescending(a => a.Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Todo>> Page(string sort = "Id", bool desc = true, int skip = 0, int take = 10)
        {
            try
            {
                IEnumerable<Todo> list = await _uow.TodoRepository.PageAsync(sort, desc, skip, take, _includes);
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Todo>> Page(string sort = "Id", bool desc = true, string param = "", int skip = 0, int take = 10)
        {
            try
            {
                IEnumerable<Todo> list;
                if (!string.IsNullOrEmpty(param))
                {
                    Expression<Func<Todo, bool>> condition = (a => a.Desc.Contains(param) || a.Name.Contains(param));
                    list = await _uow.TodoRepository.PageAsync(sort, desc, condition, skip, take, _includes);
                }
                else
                    list = await _uow.TodoRepository.PageAsync(sort, desc, skip, take, _includes);
                
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Todo>> Search(string sort = "Id", bool desc = true, string param = "")
        {
            try
            {
                IEnumerable<Todo> list;
                if (!string.IsNullOrEmpty(param))
                {
                    Expression<Func<Todo, bool>> predicate = (a => a.Desc.Contains(param) || a.Name.Contains(param));
                    return await _uow.TodoRepository.SearchByAsync(sort, desc, predicate, _includes);
                }
                return null;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Todo>> GetByStatus(string sort = "Id", bool desc = true, int status = 0)
        {
            try
            {
                IEnumerable<Todo> list;
                if (status >= 0)
                {
                    Expression<Func<Todo, bool>> predicate = (a => a.Status == status);
                    return await _uow.TodoRepository.SearchByAsync(sort, desc, predicate, _includes);
                }
                return await _uow.TodoRepository.GetAllAsync(_includes);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Todo Get By Id Method
        public async Task<Todo> Get(int? todoId)
        {
            try
            {
                _cache.TryGetValue(cacheKey, out todoCacheList);
                if (todoCacheList != null)
                {
                    var list = todoCacheList.ToList();
                    var item = list.SingleOrDefault(a => a.Id == todoId);
                    if (item != null)
                    {
                        return item;
                    }
                }
                var result = await _uow.TodoRepository.GetByIdAsync(todoId);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
