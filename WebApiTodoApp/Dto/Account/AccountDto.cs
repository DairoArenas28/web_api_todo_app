using WebApiTodoApp.Dto.Assignment;
using WebApiTodoApp.Enum;

namespace WebApiTodoApp.Dto.Account
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public UserRole Role { get; set; }

        // Opcional: incluir tareas del usuario
        public List<AssignmentDto>? Assignments { get; set; }
    }
}
