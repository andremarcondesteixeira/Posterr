using Posterr.Core.Domain.Users;

namespace Posterr.Core.Application.Interfaces;

public interface IUserRepository
{
    Task<IUser?> FindByUsername(string username);
}
