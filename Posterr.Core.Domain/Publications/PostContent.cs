using Posterr.Core.Domain.Publications.Exceptions;

namespace Posterr.Core.Domain.Publications;

public sealed record PostContent
{
    public const int MAX_LENGTH = 777;

    public string Value { get; }

    public PostContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new EmptyPostContentException("The post content must not be empty.");
        }

        if (content.Length > MAX_LENGTH)
        {
            throw new MaxPostContentLengthExceededException(
                $"The post content is limited to {MAX_LENGTH} characters. Got {content.Length} instead."
            );
        }

        Value = content;
    }
}
