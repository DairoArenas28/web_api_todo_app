namespace WebApiTodoApp.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public int StateId { get; set; }
        public State? State { get; set; }

        // Clave foránea para User
        public int UserId { get; set; }
        public User? User { get; set; }
    }

    /*public enum AssignmentStatus
    {
        Pendiente,
        EnProceso,
        Completado
    }*/
}
