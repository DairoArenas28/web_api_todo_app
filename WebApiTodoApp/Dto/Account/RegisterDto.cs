using WebApiTodoApp.Enum;

namespace WebApiTodoApp.Dto.Account
{
    public class RegisterDto
    {
        public string? UserName { get; set; }
        public string? PasswordHash { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
    }

}
