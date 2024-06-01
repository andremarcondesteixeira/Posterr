using Posterr.Core.Boundaries.EntitiesInterfaces;

namespace Posterr.Core.Boundaries.Persistence;

public interface IUsersRepository
{
    IUser? FindByUsername(string username);
}
