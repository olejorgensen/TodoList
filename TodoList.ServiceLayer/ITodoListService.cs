namespace TodoList.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Model;

public interface ITodoListService
{
    Task<List<Model.TodoList>> GetTodoListsByFilter(string? filter = null, bool ascending = true);
    Task<bool> DeleteTodoList(string name);
    Task<TodoList> UpsertTodoList(string name, string? description, bool isDone = false);
}
