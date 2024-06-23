using Posterr.Core.Shared.EntitiesInterfaces;

namespace Posterr.Core.Shared.PersistenceInterfaces;

public interface IUsersRepository
{
    IList<IUser> All();
    IUser? FindById(long id);
    IUser? FindByUsername(string username);
}
