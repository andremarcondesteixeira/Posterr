using Posterr.Core.Domain.Boundaries.Persistence;

namespace Posterr.Core.Application.Boundaries.Persistence;

public interface IUsersRepository
{
    Task<IUser?> FindByUsername(string username);
}
