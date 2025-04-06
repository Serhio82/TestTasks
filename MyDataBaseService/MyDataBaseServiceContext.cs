using Microsoft.EntityFrameworkCore;
using MyDataBaseService.Entities;


namespace MyDataBaseService
{
    public class MyDataBaseServiceContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<Department> Departments { get; set; } = null!;

        public MyDataBaseServiceContext(DbContextOptions<MyDataBaseServiceContext> options) : base(options) { }
        
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
