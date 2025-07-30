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
        public DbSet<Calendar> Calendars { get; set; }
        
                // Configuraciones adicionales (opcional por ahora)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Example: Unique Email constraint (redundante si ya lo hiciste con data annotations)
            modelBuilder.Entity<User>()
                        .HasIndex(u => u.Email)
                        .IsUnique();
            
            // Configurar relación opcional entre Subject y User
            modelBuilder.Entity<Subject>()
                .HasOne(s => s.User)
                .WithMany(u => u.Subjects)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.SetNull); // En lugar de Cascade
        }

    }
}