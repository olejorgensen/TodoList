namespace TodoList.Model
{
    public class TodoList
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Untitled";
        public string? Description { get; set; } = null;
        public bool IsDone { get; set; } = false;
    }
}