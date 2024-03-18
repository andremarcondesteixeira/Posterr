﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Posterr.Infrastructure.Persistence.DbEntities;

[Table("Users")]
[Index(nameof(Username), IsUnique = true)]
public class UserDbEntity : BaseDbEntity
{
    public required string Username { get; set; }
}
