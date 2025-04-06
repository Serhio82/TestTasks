using Microsoft.EntityFrameworkCore;
using MyService.Entities;

namespace MyService
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<Department> Departments { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>()
                .HasMany(p => p.Employees)
                .WithOne(e => e.Post)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
