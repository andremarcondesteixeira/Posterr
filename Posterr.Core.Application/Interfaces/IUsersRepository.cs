using Posterr.Core.Domain.PersistenceBoundaryInterfaces;

namespace Posterr.Core.Application.Interfaces;

public interface IUsersRepository
{
    Task<IUser?> FindByUsername(string username);
}
