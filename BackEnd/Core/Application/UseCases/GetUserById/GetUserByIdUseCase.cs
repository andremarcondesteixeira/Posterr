using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Core.Boundaries.Persistence;
using Posterr.Core.Shared.Exceptions;

namespace Posterr.Core.Application.UseCases.GetUserById;

public class GetUserByIdUseCase(IUsersRepository usersRepository) : IUseCase<long, IUser>
{
    public IUser Run(long userId)
    {
        return usersRepository.FindById(userId)
            ?? throw new UserNotFoundException(userId);
    }
}