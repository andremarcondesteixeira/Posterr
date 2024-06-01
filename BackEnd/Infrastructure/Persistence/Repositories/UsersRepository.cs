using Posterr.Core.Boundaries.Persistence;
using Posterr.Core.Boundaries.EntitiesInterfaces;

namespace Posterr.Infrastructure.Persistence.Repositories;

public class UsersRepository(ApplicationDbContext dbContext) : IUsersRepository
{
    public IUser? FindByUsername(string username)
    {
        var queryResult = dbContext.Users.Where(u => u.Username == username);

        if (!queryResult.Any())
        {
            return null;
        }

        return queryResult.First().ToIUser();
    }
}
