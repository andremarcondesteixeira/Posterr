using Posterr.Core.Boundaries.Configuration;
using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Core.Domain.Entities.Exceptions;

namespace Posterr.Core.Domain.Entities.Publications.Exceptions;

public class MaxAllowedDailyPublicationsByUserExceededException(IUser Author, IDomainConfig DomainConfig)
    : DomainException(
        $"The user {Author.Username} is not allowed to make more than {DomainConfig.MaxAllowedDailyPublicationsByUser} publications in a single day."
    )
{
}
