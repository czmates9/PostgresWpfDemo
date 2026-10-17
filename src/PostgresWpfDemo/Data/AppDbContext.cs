using Microsoft.EntityFrameworkCore;
using PostgresWpfDemo.Models;

namespace PostgresWpfDemo.Data;

public sealed class AppDbContext(
    DbContextOptions<AppDbContext> options)
    : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasKey(user => user.Id);

            entity.Property(user => user.Id)
                .HasColumnName("id");

            entity.Property(user => user.Name)
                .HasColumnName("name");

            entity.Property(user => user.Email)
                .HasColumnName("email");

            entity.Property(user => user.CreatedAt)
                .HasColumnName("created_at");
        });
    }
}