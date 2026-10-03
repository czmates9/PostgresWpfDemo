using Microsoft.EntityFrameworkCore;
using PostgresWpfDemo.Models;

namespace PostgresWpfDemo.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string? connectionString =
                Environment.GetEnvironmentVariable("POSTGRESWPFDEMO_CONNECTION");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Chybí environment variable POSTGRESWPFDEMO_CONNECTION.");
            }

            optionsBuilder.UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasColumnName("id");

                entity.Property(x => x.Name)
                    .HasColumnName("name");

                entity.Property(x => x.Email)
                    .HasColumnName("email");

                entity.Property(x => x.CreatedAt)
                    .HasColumnName("created_at");
            });
        }
    }
}