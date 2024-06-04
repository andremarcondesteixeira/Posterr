using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Core.Boundaries.Persistence;

namespace Posterr.Core.Application.UseCases.ListUsers;

public class ListUsersUseCase(IUsersRepository usersRepository) : IUseCase<IList<IUser>>
{
    public IList<IUser> Run() => usersRepository.All();
}