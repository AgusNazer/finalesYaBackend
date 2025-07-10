using Microsoft.EntityFrameworkCore;
// using finalesYaBackend.Models;

namespace finalesYaBackend.Models
{
    public class AppDbContext : DbContext
    {
        // Constructor
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets (tablas)
        public DbSet<User> Users { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Comment> Comments { get; set; }
        
                // Configuraciones adicionales (opcional por ahora)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Example: Unique Email constraint (redundante si ya lo hiciste con data annotations)
            modelBuilder.Entity<User>()
                        .HasIndex(u => u.Email)
                        .IsUnique();
        }

    }
}