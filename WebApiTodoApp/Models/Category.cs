namespace WebApiTodoApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? dateCreation { get; set; }

        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    }
}
