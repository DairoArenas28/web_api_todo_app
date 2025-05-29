using WebApiTodoApp.Models;

namespace WebApiTodoApp.Dto.Assignment
{
    public class CreateAssignmentDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int StatusId { get; set; }
    }
}
