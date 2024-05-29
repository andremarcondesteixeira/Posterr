using Posterr.Core.Boundaries.Persistence;
using Posterr.Core.Boundaries.EntitiesInterfaces;

namespace Posterr.Infrastructure.Persistence.Repositories;

public class UsersRepository(ApplicationDbContext dbContext) : IUsersRepository
{
    public Task<IUser?> FindByUsername(string username)
    {
        var queryResult = dbContext.Users.Where(u => u.Username == username);

        if (!queryResult.Any())
        {
            return Task.FromResult<IUser?>(null);
        }

        return Task.FromResult<IUser?>(queryResult.First().ToIUser());
    }
}
