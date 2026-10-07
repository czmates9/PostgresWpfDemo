using Microsoft.EntityFrameworkCore;
using PostgresWpfDemo.Data;
using PostgresWpfDemo.Models;

namespace PostgresWpfDemo.Services;

public sealed class UserService : IUserService
{
    public async Task<List<User>> GetAllAsync()
    {
        await using var db = new AppDbContext();

        return await db.Users
            .OrderBy(user => user.Id)
            .ToListAsync();
    }

    public async Task AddAsync(User user)
    {
        await using var db = new AppDbContext();

        db.Users.Add(user);

        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        await using var db = new AppDbContext();

        var existingUser = await db.Users.FindAsync(user.Id);

        if (existingUser is null)
            return;

        existingUser.Name = user.Name;

        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        await using var db = new AppDbContext();

        db.Users.Remove(user);

        await db.SaveChangesAsync();
    }
}