namespace WebApiTodoApp.Models
{
    public class Assignment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Name { get; set; }
        public string? Description { get; set; }

        public AssignmentStatus Status { get; set; } = AssignmentStatus.Pending;

        // Clave foránea para User
        public int UserId { get; set; }
        public User? User { get; set; }
    }

    public enum AssignmentStatus
    {
        Pending,
        Complete
    }
}
