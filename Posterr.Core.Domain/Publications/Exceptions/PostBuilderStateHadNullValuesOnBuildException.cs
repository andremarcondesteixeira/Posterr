using Posterr.Core.Domain.Exceptions;

namespace Posterr.Core.Domain.Publications.Exceptions;

public sealed class PostBuilderStateHadNullValuesOnBuildException(IList<string> propertiesWithNullValue)
    : DomainValidationException(
        $"The following properties are null: {string.Join(", ", propertiesWithNullValue)}."
    )
{
    public IList<string> PropertiesWithNullValue { get; } = propertiesWithNullValue;
}
