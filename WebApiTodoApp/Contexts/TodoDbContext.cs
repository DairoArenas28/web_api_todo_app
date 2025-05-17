using Microsoft.EntityFrameworkCore;
using WebApiTodoApp.Models;

namespace WebApiTodoApp.Contexts
{
    public class TodoDbContext: DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Assignment> Assignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar la relación uno-a-muchos: un User tiene muchas Assignments
            modelBuilder.Entity<User>()
                .HasMany(u => u.Assignments)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
