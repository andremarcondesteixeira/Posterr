using Posterr.Core.Boundaries.ConfigurationInterface;

namespace Posterr.Core.Shared.Exceptions;

public sealed class EmptyPostContentException(IDomainConfig domainConfig)
    : PosterrException(
        "The post content must not be empty.",
        $"Provide content having between 1 and {domainConfig.MaxPostLength} characters."
    )
{
}
