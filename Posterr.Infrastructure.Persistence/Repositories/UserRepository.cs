﻿using Posterr.Core.Application.Interfaces;
using Posterr.Core.Domain.PersistenceBoundaryInterfaces;

namespace Posterr.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    public Task<IUser?> FindByUsername(string username)
    {
        throw new NotImplementedException();
    }
}