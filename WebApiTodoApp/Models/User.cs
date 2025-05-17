namespace WebApiTodoApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Un usuario puede tener muchas tareas
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    }
}
