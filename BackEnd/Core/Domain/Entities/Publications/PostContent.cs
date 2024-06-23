using Posterr.Core.Shared.ConfigurationInterfaces;
using Posterr.Core.Shared.Exceptions;

namespace Posterr.Core.Domain.Entities.Publications;

public sealed record PostContent
{
    public string Value { get; }

    public PostContent(string content, IDomainConfig domainConfig)
    {
        ArgumentNullException.ThrowIfNull(domainConfig, nameof(domainConfig));

        if (string.IsNullOrWhiteSpace(content))
        {
            throw new EmptyPostContentException(domainConfig);
        }

        if (content.Length > domainConfig.MaxPostLength)
        {
            throw new MaxPublicationContentLengthExceededException(content.Length, domainConfig);
        }

        Value = content;
    }
}
