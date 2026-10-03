using Microsoft.EntityFrameworkCore;
using PostgresWpfDemo.Models;

namespace PostgresWpfDemo.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=PostgresWpfDemoDb;Username=postgres;Password=pro147fask");
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

                entity.Property(x => x.CreatedAt)
                    .HasColumnName("created_at");

                entity.Property(x => x.Email)
    .HasColumnName("email");
            });
        }
    }
}