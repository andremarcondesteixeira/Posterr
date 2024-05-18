﻿using Posterr.Core.Boundaries.EntitiesInterfaces;

namespace Posterr.Core.Domain.Entities;

// I decided to make the interface methods async because it will make the
// interface compatible with a wider range of persistence methods
public interface IDomainPersistencePort
{
    Task<ushort> AmountOfPublicationsMadeTodayBy(IUser author);
    Task<IPost> PublishNewPost(IUnpublishedPost unpublishedPost);
    Task<IRepost> PublishNewRepost(IUnpublishedRepost unpublishedRepost);
}