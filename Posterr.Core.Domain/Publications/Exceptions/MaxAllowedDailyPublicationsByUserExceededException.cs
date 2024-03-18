using Posterr.Core.Domain.Boundaries.Configuration;
using Posterr.Core.Domain.Boundaries.Persistence;
using Posterr.Core.Domain.Exceptions;

namespace Posterr.Core.Domain.Publications.Exceptions;

public class MaxAllowedDailyPublicationsByUserExceededException(IUser Author, IDomainConfig DomainConfig)
    : DomainException(
        $"The user {Author.Username} is not allowed to make more than {DomainConfig.MaxAllowedDailyPublicationsByUser} publications in a single day."
    )
{
}
