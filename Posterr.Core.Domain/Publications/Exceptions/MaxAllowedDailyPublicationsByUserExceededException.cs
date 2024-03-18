using Posterr.Core.Domain.Exceptions;
using Posterr.Core.Domain.PersistenceBoundaryInterfaces;

namespace Posterr.Core.Domain.Publications.Exceptions;

public class MaxAllowedDailyPublicationsByUserExceededException(IUser Author, IDomainConfig DomainConfig)
    : DomainException(
        $"The user {Author.Username} is not allowed to make more than {DomainConfig.MaxAllowedDailyPublicationsByUser} publications in a single day."
    )
{
}
