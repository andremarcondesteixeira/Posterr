using Posterr.Core.Shared.EntitiesInterfaces;
using Posterr.Core.Shared.PersistenceInterfaces;

namespace Posterr.Core.Application.UseCases.ListUsers;

public class ListUsersUseCase(IUsersRepository usersRepository) : IUseCase<IList<IUser>>
{
    public IList<IUser> Run() => usersRepository.All();
}