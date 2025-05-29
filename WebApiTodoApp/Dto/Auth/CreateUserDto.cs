using WebApiTodoApp.Enum;

namespace WebApiTodoApp.Dto.Auth
{
    public class CreateUserDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.User; // Por defecto "User"
    }
}
