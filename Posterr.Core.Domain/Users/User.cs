namespace Posterr.Core.Domain.Users;

public sealed record User
{
    public Username Username { get; }

    public User(string username)
    {
        // Here the Username value object performs the validation.
        // I'm implementing the concept of making illegal states unrepresentable.
        // Doing validation in the constructor is, in my opinion, the strongest way of making illegal states unrepresentable in C#.
        Username = new Username(username);
    }
}
