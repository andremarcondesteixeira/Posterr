using Posterr.Core.Domain.PersistenceBoundaryInterfaces;

namespace Posterr.Core.Application.Interfaces;

public interface IUserRepository
{
    Task<IUser?> FindByUsername(string username);
}
