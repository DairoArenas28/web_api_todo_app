using WebApiTodoApp.Enum;

namespace WebApiTodoApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.User;
        // Un usuario puede tener muchas tareas
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    }
}
