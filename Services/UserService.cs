using Microsoft.EntityFrameworkCore;
using PostgresWpfDemo.Data;
using PostgresWpfDemo.Models;

namespace PostgresWpfDemo.Services
{
    public class UserService
    {
        public async Task<List<User>> GetAllAsync()
        {
            using var db = new AppDbContext();

            return await db.Users
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            using var db = new AppDbContext();

            db.Users.Add(user);

            await db.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            using var db = new AppDbContext();

            db.Users.Update(user);

            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            using var db = new AppDbContext();

            db.Users.Remove(user);

            await db.SaveChangesAsync();
        }
    }
}