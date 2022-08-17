namespace TodoList.DB;

using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Model;

public static class SqliteDataAccess
{
    #region SQL Statements

    public static readonly string DeleteTodoListByNameStatement = "delete from TodoList where Name=@Name";
    public static readonly string SelectTodoListsStatement = "select Id, Name, Description, IsDone from TodoList order by Name";
    public static readonly string SelectTodoListByNameStatement = "select Id, Name, Description, IsDone from TodoList where Name=@Name";
    public static readonly string SelectFilteredCategoriesStatement = "select Id, Name, Description, IsDone from TodoList where Name like @Filter or Description like @Filter order by Name";
    public static readonly string InsertTodoListStatement = "insert into TodoList (Name, Description, IsDone) values (@Name, @Description, @IsDone)";
    public static readonly string UpdateTodoListStatement = "update TodoList set Name=@Name, Description=@Description, IsDone=@IsDone where Id=@Id";
    public static readonly string FinishTodoListStatement = "update TodoList set IsDone=1 where Id=@Id";

    #endregion

    #region TodoList

    private static async Task<TodoList?> GetTodoListByNameAsync(IDbConnection cnn, DynamicParameters parameters)
    {
#if TRACE
        Trace.WriteLine($"{DateTime.UtcNow:s}: {nameof(SqliteDataAccess)}.GetTodoListByNameAsync");
#endif
        var output = await cnn.QueryAsync<TodoList>(SelectTodoListByNameStatement, parameters);
        var first = output.FirstOrDefault();
        return first;
    }

    public static async Task<List<TodoList>> GetTodoListAsync(string? filter, bool? ascending)
    {
        return await GetTodoListAsync(filter, ascending ?? true);
    }

    private static async Task<List<TodoList>> GetTodoListAsync(string? filter = null, bool ascending = true)
    {
#if TRACE
        Trace.WriteLine($"{DateTime.UtcNow:s.fff}: {nameof(SqliteDataAccess)}.GetTodoListAsync");
#endif

        var connectionString = LoadConnectionString();
        using IDbConnection cnn = new SQLiteConnection(connectionString);
        cnn.Open();

        var parameters = new DynamicParameters();
        if (!string.IsNullOrWhiteSpace(filter))
        {
            parameters.AddDynamicParams(new { Filter = $"%{filter}%" });
        }

        var ordering = ascending ? "ASC" : "DESC";
        var statement = string.IsNullOrWhiteSpace(filter)
            ? $"{SelectTodoListsStatement} {ordering}"
            : $"{SelectFilteredCategoriesStatement} {ordering}";

        var output = await cnn.QueryAsync<TodoList>(statement, parameters);
        return output.ToList();
    }

    public static async Task<bool> DeleteTodoListAsync(string name)
    {
        var connectionString = LoadConnectionString();
        using IDbConnection cnn = new SQLiteConnection(connectionString);
        cnn.Open();

        var rowsAffected = await cnn.ExecuteAsync(DeleteTodoListByNameStatement, new DynamicParameters(new { Name = name }));

        return rowsAffected > 0;
    }

    public static async Task<TodoList> UpsertTodoListAsync(string name, string? description, bool isDone = false)
    {
        var connectionString = LoadConnectionString();
        using IDbConnection cnn = new SQLiteConnection(connectionString);
        cnn.Open();

        // TODO begin, commit, rollback transction

        var parameters = new DynamicParameters(new { Name = name, Description = description, IsDone = isDone });
        var todoList = await GetTodoListByNameAsync(cnn, parameters);
        if (todoList is null)
        {
            await cnn.ExecuteAsync(InsertTodoListStatement, parameters);
        }
        else
        {
            parameters.AddDynamicParams(new { todoList.Id });
            await cnn.ExecuteAsync(UpdateTodoListStatement, parameters);
        }
        todoList = await GetTodoListByNameAsync(cnn, parameters);

        return todoList;
    }


    #endregion

    #region ConnectionString

    private static readonly Dictionary<string, string> cachedConnectionStrings = new();

    private static string LoadConnectionString(string id = "Default")
    {
        lock (cachedConnectionStrings)
        {
            if (cachedConnectionStrings.ContainsKey(id) == false)
            {
                cachedConnectionStrings.Add(id, ConfigurationManager.ConnectionStrings[id].ConnectionString);
            }
            return cachedConnectionStrings[id];
        }
    }

    #endregion
}
