using Microsoft.EntityFrameworkCore;
using Posterr.Core.Application.Interfaces;
using Posterr.Core.Domain.Boundaries.Persistence;

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
