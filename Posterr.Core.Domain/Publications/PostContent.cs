using Posterr.Core.Domain.Publications.Exceptions;

namespace Posterr.Core.Domain.Publications;

public sealed record PostContent
{
    public string Value { get; }

    public PostContent(string content, IDomainConfig domainConfig)
    {
        ArgumentNullException.ThrowIfNull(domainConfig, nameof(domainConfig));

        if (string.IsNullOrWhiteSpace(content))
        {
            throw new EmptyPostContentException("The post content must not be empty.");
        }

        if (content.Length > domainConfig.MaxPostLength)
        {
            throw new MaxPostContentLengthExceededException(
                $"The post content is limited to {domainConfig.MaxPostLength} characters. Got {content.Length} instead."
            );
        }

        Value = content;
    }
}
