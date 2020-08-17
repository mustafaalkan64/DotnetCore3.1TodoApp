namespace Todo_App.DAL.EfDbContext
{
    using Microsoft.EntityFrameworkCore;
    using Todo_App.Domain.Entities;

    public partial class TodoListContext : DbContext
    {

        public TodoListContext(DbContextOptions<TodoListContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Todo> TodoList { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .HasMany(e => e.Todo)
                .WithOne(e => e.CreatedByUser)
                .HasForeignKey(e => e.CreatedBy);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Todo1)
                .WithOne(e => e.UpdatedByUser)
                .HasForeignKey(e => e.UpdatedBy);
        }
    }
}
