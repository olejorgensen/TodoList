namespace TodoList.Services;

using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Model;
using TodoList.Repositories;

public class TodoListService : ITodoListService
{
    readonly ITodoListRepository _todoListRepository;
    readonly ILogger<ITodoListService> _logger;

    public TodoListService(ITodoListRepository todoListRepository, ILogger<ITodoListService> logger)
    {
        _todoListRepository = todoListRepository ?? throw new ArgumentNullException(nameof(todoListRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<List<TodoList>> GetTodoListsByFilter(string? filter = null, bool ascending = true)
    {
        return _todoListRepository.GetTodoListAsync(filter, ascending);
    }

    public Task<bool> DeleteTodoList(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentOutOfRangeException(nameof(name));
        }
        return _todoListRepository.DeleteTodoListAsync(name);
    }

    public Task<TodoList> UpsertTodoList(string name, string? description, bool isDone = false)
    {
        return _todoListRepository.UpsertTodoListAsync(name, description, isDone);
    }
}
