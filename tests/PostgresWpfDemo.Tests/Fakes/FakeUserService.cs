using PostgresWpfDemo.Models;
using PostgresWpfDemo.Services;

namespace PostgresWpfDemo.Tests.Fakes;

public sealed class FakeUserService : IUserService
{
    public List<User> Users { get; } = [];

    public Task<List<User>> GetAllAsync()
        => Task.FromResult(Users.ToList());

    public Task AddAsync(User user)
    {
        Users.Add(user);

        return Task.CompletedTask;
    }

    public Task UpdateAsync(User user)
    {
        var existingUser = Users.FirstOrDefault(
            existingUser => existingUser.Id == user.Id);

        if (existingUser is not null)
            existingUser.Name = user.Name;

        return Task.CompletedTask;
    }

    public Task DeleteAsync(User user)
    {
        Users.RemoveAll(
            existingUser => existingUser.Id == user.Id);

        return Task.CompletedTask;
    }
}