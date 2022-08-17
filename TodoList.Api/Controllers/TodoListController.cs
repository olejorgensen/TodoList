using Microsoft.AspNetCore.Mvc;
using TodoList.Services;

namespace TodoList.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoListController : ControllerBase
    {
        private readonly ITodoListService _service;
        private readonly ILogger<TodoListController> _logger;

        public TodoListController(ITodoListService service, ILogger<TodoListController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteTodoList(Model.TodoList todoList)
        {
            _logger.Log(LogLevel.Information, "DeleteTodoList");

            var wasDeleted = await _service.DeleteTodoList(todoList.Name);

            return AcceptedAtAction(nameof(DeleteTodoList), new { Success = wasDeleted });
        }

        [HttpPost]
        public async Task<ActionResult<Model.TodoList>> PostTodoList(Model.TodoList todoList)
        {
            _logger.Log(LogLevel.Information, "PostTodoList");

            var updatedOrCreated = await _service.UpsertTodoList(todoList.Name, todoList.Description, todoList.IsDone);

            return CreatedAtAction(nameof(PostTodoList), new { id = updatedOrCreated.Id, updatedOrCreated });
        }

        [HttpGet(Name = "todos?q={q}&asc={asc}")]
        public async Task<IEnumerable<Model.TodoList>> GetAsync(string? q, bool asc = true)
        {
            _logger.Log(LogLevel.Information, "GetAsync filtered");

            if (q is not null)
            {
                if (q.Length > 100)
                {
                    q = q.Substring(0, 100).Trim();
                }
                q = Uri.UnescapeDataString(q).Trim();
            }

            return await _service.GetTodoListsByFilter(q, asc);
        }
    }
}