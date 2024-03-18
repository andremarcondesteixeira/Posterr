using Posterr.Core.Domain.Boundaries.Persistence;

namespace Posterr.Core.Application.Interfaces;

public interface IUsersRepository
{
    Task<IUser?> FindByUsername(string username);
}
