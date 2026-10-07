using PostgresWpfDemo.Models;

namespace PostgresWpfDemo.Services;

public interface IUserService
{
    Task<List<User>> GetAllAsync();

    Task AddAsync(User user);

    Task UpdateAsync(User user);

    Task DeleteAsync(User user);
}