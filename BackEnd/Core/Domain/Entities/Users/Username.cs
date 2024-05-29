using Posterr.Core.Shared.Exceptions;
using System.Text.RegularExpressions;

namespace Posterr.Core.Domain.Entities.Users;

public partial record Username
{
    public string Value { get; }

    // Borrowed from DDD, I use the concept in which value objects should own their validation rules.
    // This makes illegal states unprepresentable, and doing so in the constructor is the strongest way of doing it
    public Username(string value)
    {
        var regex = UsernameRegex();

        if (string.IsNullOrWhiteSpace(value) || !regex.IsMatch(value))
        {
            throw new InvalidUsernameException(value);
        }

        Value = value;
    }

    [GeneratedRegex("^[a-zA-Z0-9]+$")]
    private static partial Regex UsernameRegex();

    public override string ToString() => Value;
}
