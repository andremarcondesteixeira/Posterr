using Posterr.Core.Boundaries.EntitiesInterfaces;

namespace Posterr.Core.Boundaries.Persistence;

public interface IUsersRepository
{
    Task<IUser?> FindByUsername(string username);
}
