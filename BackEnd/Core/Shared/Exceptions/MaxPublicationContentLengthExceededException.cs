using Posterr.Core.Shared.ConfigurationInterfaces;

namespace Posterr.Core.Shared.Exceptions;

public sealed class MaxPublicationContentLengthExceededException(int contentLength, IDomainConfig domainConfig)
    : PosterrException(
        $"The publication content is limited to {domainConfig.MaxPostLength} characters. Got {contentLength} instead.",
        $"Provide content having between 1 and {domainConfig.MaxPostLength} characters."
    )
{
}