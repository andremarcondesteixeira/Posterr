using Posterr.Core.Domain.Entities.Exceptions;

namespace Posterr.Core.Domain.Entities.Publications.Exceptions;

public sealed class PostBuilderStateHadNullValuesOnBuildException(IList<string> propertiesWithNullValue)
    : DomainValidationException(
        $"The following properties are null: {string.Join(", ", propertiesWithNullValue)}."
    )
{
    public IList<string> PropertiesWithNullValue { get; } = propertiesWithNullValue;
}
