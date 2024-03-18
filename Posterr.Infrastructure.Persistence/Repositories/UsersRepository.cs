using Posterr.Core.Application.Interfaces;
using Posterr.Core.Domain.Boundaries.Persistence;

namespace Posterr.Infrastructure.Persistence.Repositories;

public class UsersRepository : IUsersRepository
{
    public Task<IUser?> FindByUsername(string username)
    {
        throw new NotImplementedException();
    }
}
