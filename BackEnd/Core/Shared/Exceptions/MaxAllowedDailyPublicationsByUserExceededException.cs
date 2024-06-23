using Posterr.Core.Shared.ConfigurationInterfaces;
using Posterr.Core.Shared.EntitiesInterfaces;

namespace Posterr.Core.Shared.Exceptions;

public class MaxAllowedDailyPublicationsByUserExceededException(IUser author, IDomainConfig domainConfig)
    : PosterrException(
        $"The user {author.Username} is not allowed to make more than {domainConfig.MaxAllowedDailyPublicationsByUser} publications in a single day.",
        "Wait until tomorrow to make new publications."
    )
{
}
