namespace TodoList.Repositories;

using Microsoft.Extensions.Logging;
using TodoList.DB;
using TodoList.Model;

public class TodoListRepository : ITodoListRepository
{
    readonly ILogger<ITodoListRepository> _logger;

    public TodoListRepository(ILogger<ITodoListRepository> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger)); 
    }

    public Task<List<Model.TodoList>> GetTodoListAsync(string? filter, bool? ascending)
    {
        // TODO: log
        return SqliteDataAccess.GetTodoListAsync(filter, ascending);
    }

    public Task<bool> DeleteTodoListAsync(string name)
    {
        // TODO: log
        return SqliteDataAccess.DeleteTodoListAsync(name);
    }
    public Task<TodoList> UpsertTodoListAsync(string name, string? description, bool isDone = false)
    {
        // TODO: log
        return SqliteDataAccess.UpsertTodoListAsync(name, description, isDone);
    }
}
