using WebApiTodoApp.Models;

namespace WebApiTodoApp.Dto.Assignment
{
    public class AssignmentDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int StatusId { get; set; }

        public int CategoryId { get; set; }

        // Opcional
        public int UserId { get; set; }
    }
}
