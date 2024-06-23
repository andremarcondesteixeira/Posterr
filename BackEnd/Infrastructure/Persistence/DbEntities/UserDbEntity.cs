using Microsoft.EntityFrameworkCore;
using Posterr.Core.Shared.EntitiesInterfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Posterr.Infrastructure.Persistence.DbEntities;

[Table("Users")]
[Index(nameof(Username), IsUnique = true)]
public class UserDbEntity : BaseDbEntity
{
    public required string Username { get; set; }

    public IUser ToIUser() => new User(Id, Username);

    public record User(long Id, string Username) : IUser;
}

