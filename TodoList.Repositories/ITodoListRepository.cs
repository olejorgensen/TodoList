namespace TodoList.Repositories;

public interface ITodoListRepository
{
    Task<List<Model.TodoList>> GetTodoListAsync(string? filter, bool? ascending);
    Task<bool> DeleteTodoListAsync(string name);
    Task<Model.TodoList> UpsertTodoListAsync(string name, string? description, bool isDone = false);
}
