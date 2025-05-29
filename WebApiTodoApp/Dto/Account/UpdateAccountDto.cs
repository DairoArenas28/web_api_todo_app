using WebApiTodoApp.Enum;

namespace WebApiTodoApp.Dto.Account
{
    public class UpdateAccountDto
    {
        public string? UserName { get; set; }

        public string? Password { get; set; }
        public UserRole Role { get; set; }
    }
}
