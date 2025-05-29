namespace WebApiTodoApp.Models
{
    public class State
    {
        public int Id { get; set; }
        public string? nameState { get; set; }

        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    }
}
