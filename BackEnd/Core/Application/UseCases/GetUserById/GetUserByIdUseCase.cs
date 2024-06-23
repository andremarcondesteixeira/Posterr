using Posterr.Core.Shared.EntitiesInterfaces;
using Posterr.Core.Shared.Exceptions;
using Posterr.Core.Shared.PersistenceInterfaces;

namespace Posterr.Core.Application.UseCases.GetUserById;

public class GetUserByIdUseCase(IUsersRepository usersRepository) : IUseCase<long, IUser>
{
    public IUser Run(long userId)
    {
        return usersRepository.FindById(userId)
            ?? throw new UserNotFoundException(userId);
    }
}