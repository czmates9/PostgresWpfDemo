using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PostgresWpfDemo.Data;
using PostgresWpfDemo.Models;

namespace PostgresWpfDemo.Services;

public sealed class UserService : IUserService
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IDbContextFactory<AppDbContext> dbContextFactory,
        ILogger<UserService> logger)
    {
        _dbContextFactory = dbContextFactory;
        _logger = logger;
    }

    public async Task<List<User>> GetAllAsync()
    {
        try
        {
            await using var db =
                await _dbContextFactory.CreateDbContextAsync();

            return await db.Users
                .OrderBy(user => user.Id)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to load users from database.");

            throw;
        }
    }

    public async Task AddAsync(User user)
    {
        try
        {
            await using var db =
                await _dbContextFactory.CreateDbContextAsync();

            db.Users.Add(user);

            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to add user {UserName}.",
                user.Name);

            throw;
        }
    }

    public async Task UpdateAsync(User user)
    {
        try
        {
            await using var db =
                await _dbContextFactory.CreateDbContextAsync();

            var existingUser =
                await db.Users.FindAsync(user.Id);

            if (existingUser is null)
            {
                _logger.LogWarning(
                    "User {UserId} was not found for update.",
                    user.Id);

                return;
            }

            existingUser.Name = user.Name;

            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to update user {UserId}.",
                user.Id);

            throw;
        }
    }

    public async Task DeleteAsync(User user)
    {
        try
        {
            await using var db =
                await _dbContextFactory.CreateDbContextAsync();

            db.Users.Remove(user);

            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to delete user {UserId}.",
                user.Id);

            throw;
        }
    }
}