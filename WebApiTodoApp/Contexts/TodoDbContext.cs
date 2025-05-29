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
        public DbSet<Category> Categories { get; set; }
        public DbSet<State> States { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar la relación uno-a-muchos: un User tiene muchas Assignments
            modelBuilder.Entity<User>()
                .HasMany(u => u.Assignments)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>()
                .HasMany(u => u.Assignments)
                .WithOne(a => a.Category)
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<State>()
             .HasMany(u => u.Assignments)
             .WithOne(a => a.State)
             .HasForeignKey(a => a.StateId)
             .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
