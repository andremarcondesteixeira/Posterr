using Posterr.Core.Shared.EntitiesInterfaces;

namespace Posterr.Core.Domain.Entities.Users;

// When seeing things from the domain model perspective, a User could be perfectly identified by it's unique username.
// Because of this, I decided not to create a BaseEntity record with an Id and other commons fields such as CreatedDate.
// Beyond this, for the scope of this test, I don't care about the date when a User was created.
public sealed record User : IUser
{
    private readonly Username _username;

    public long Id { get; }
    public string Username { get => _username.Value; }

    public User(long id, string username)
    {
        // Here, the Username value object performs the validation.
        // I'm implementing the concept of making illegal states unrepresentable.
        // Doing validation in the constructor is, in my opinion, the strongest way of making illegal states unrepresentable in C#.
        _username = new Username(username);
        Id = id;
    }
}
