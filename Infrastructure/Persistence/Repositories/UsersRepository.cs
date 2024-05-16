using Microsoft.EntityFrameworkCore;
using Posterr.Core.Boundaries.Persistence;
using Posterr.Core.Boundaries.EntitiesInterfaces;

namespace Posterr.Infrastructure.Persistence.Repositories;

public class UsersRepository(ApplicationDbContext dbContext) : IUsersRepository
{
    public Task<IUser?> FindByUsername(string username)
    {
        var user = dbContext.Users.Where(u => u.Username == username).First();

        if (user is null)
        {
            return Task.FromResult<IUser?>(null);
        }

        return Task.FromResult<IUser?>(user.ToIUser());
    }
}
