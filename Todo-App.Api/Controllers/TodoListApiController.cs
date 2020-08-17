using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Todo_App.Business.Validations;
using Todo_App.Core.Consts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Todo_App.Business.Abstract;
using Todo_App.Business.Models;

namespace ToDo_App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = RoleType.User, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TodoListApiController : BaseController
    {
        private ITodoService _todoService;
        private IMapper _mapper;
        public TodoListApiController(ITodoService todoService, 
            IMapper mapper)
        {
            _todoService = todoService;
            _mapper = mapper;
        }
        // GET: api/TodoList
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var toDoList = await _todoService.Get();
                if (toDoList == null)
                    return NotFound();

                return Ok(toDoList);

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("paging")]
        public async Task<IActionResult> Page(string sort= "Id", bool desc = true, int skip = 0, int take = 10)
        {
            try
            {
                var toDoList = await _todoService.Page(sort, desc, skip, take);
                if (toDoList == null)
                    return NotFound();

                return Ok(toDoList);

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string sort = "Id", bool desc = true, string searchTerm = "")
        {
            try
            {
                if(string.IsNullOrEmpty(searchTerm))
                    return NotFound();
                
                var toDoList = await _todoService.Search(sort, desc, searchTerm);
                if (toDoList == null)
                    return NotFound();

                return Ok(toDoList);

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // GET: api/TodoList/5
        [HttpGet]
        [Route("getById")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var toDo = await _todoService.Get(id);
                if(toDo == null)
                    return NotFound();
                
                return Ok(toDo);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        [HttpGet]
        [Route("getByStatusId")]
        public async Task<IActionResult> GetByStatusId(string sort = "Id", bool desc = true, int statusId = 0)
        {
            try
            {
                var toDo = await _todoService.GetByStatus(sort, desc, statusId);
                if (toDo == null)
                    return NotFound();

                return Ok(toDo);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // POST: api/TodoList
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TodoDto todoModel)
        {
            try
            {
                var errors = ValidatorUtility.FluentValidate(new TodoValidator(), todoModel);
                if (!string.IsNullOrEmpty(errors))
                    return BadRequest(errors);

                todoModel.CreatedBy = GetUserId();
                todoModel.CreateDate = DateTime.Now;
                var todoId = await _todoService.Add(todoModel);
                if (todoId > 0)
                    return Ok(todoId);
                else
                    return BadRequest("An Error Occured While Creating New Todo");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/TodoList/5
        [HttpPut]
        public async Task<IActionResult> Put(int? id, [FromBody]TodoDto todoModel)
        {
            try
            {
                if (id == null)
                    return BadRequest();

                // Validate TodoDTO model through Fluent Validation
                var errors = ValidatorUtility.FluentValidate(new TodoValidator(), todoModel);
                if (!string.IsNullOrEmpty(errors))
                    return BadRequest(errors);

                var result = await _todoService.Update(id, todoModel, GetUserId());
                if (result.Status)
                    return Ok();
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        [Route("updateStatus")]
        public async Task<IActionResult> Put(int? id, int status = 0)
        {
            try
            {
                if (id == null)
                    return BadRequest();

                var result = await _todoService.UpdateStatus(id, status, GetUserId());
                if (result.Status)
                    return Ok();
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                    return BadRequest();

                var result = await _todoService.Delete(id);
                if (result.Status)
                    return Ok();
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
