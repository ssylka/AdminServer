using Microsoft.EntityFrameworkCore;

namespace WebServer.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options):base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Models.Entities.User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }


        public DbSet<Models.Entities.User> Users { get; set; }
    }
}
