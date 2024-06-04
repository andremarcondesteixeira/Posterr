using Posterr.Core.Boundaries.EntitiesInterfaces;

namespace Posterr.Core.Boundaries.Persistence;

public interface IUsersRepository
{
    IList<IUser> All();
    IUser? FindById(long id);
    IUser? FindByUsername(string username);
}
