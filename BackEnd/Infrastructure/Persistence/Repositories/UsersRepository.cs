﻿using Posterr.Core.Shared.EntitiesInterfaces;
using Posterr.Core.Shared.PersistenceInterfaces;

namespace Posterr.Infrastructure.Persistence.Repositories;

public class UsersRepository(ApplicationDbContext dbContext) : IUsersRepository
{
    public IList<IUser> All() => [.. dbContext.Users.OrderBy(u => u.Username).Select(u => u.ToIUser())];

    public IUser? FindById(long id) => dbContext.Users.Find(id)?.ToIUser();

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
