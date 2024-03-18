﻿using FakeItEasy;
using Posterr.Core.Application.Interfaces;
using Posterr.Core.Domain;
using Posterr.Core.Domain.PersistenceBoundaryInterfaces;

namespace Posterr.Core.Application.Tests;

record PresumeThat(
    IUserRepository UserRepository,
    IPublicationRepository PublicationRepository,
    IDomainPersistencePort DomainPersistenceAdapter,
    IDomainConfig DomainConfig
)
{
    public static PresumeThat ItWorks() => new(The.UserRepository(),
                                               The.PublicationRepository(),
                                               The.DomainPersistenceAdapter(),
                                               The.DomainConfigForTests());

    public void DomainPersistencePortSuccessfullyPublishesPost(IUnpublishedPost unpublishedPost, IPost post)
    {
        A.CallTo(() => DomainPersistenceAdapter.PublishNewPost(
        A<IUnpublishedPost>.That.Matches(
        x => x.Author.Username == unpublishedPost.Author.Username && x.Content == unpublishedPost.Content
            )
        )).Returns(post);
    }

    public void DomainPersistencePortSuccessfullyPublishesRepost(IUnpublishedRepost unpublishedRepost, IRepost repost)
    {
        A.CallTo(() => DomainPersistenceAdapter.PublishNewRepost(
        A<IUnpublishedRepost>.That.Matches(r =>
                r.Author.Username == unpublishedRepost.Author.Username
                && r.OriginalPost.Id == unpublishedRepost.OriginalPost.Id
                && r.OriginalPost.Author.Username == unpublishedRepost.OriginalPost.Author.Username
                && r.OriginalPost.PublicationDate == unpublishedRepost.OriginalPost.PublicationDate
                && r.OriginalPost.Content == unpublishedRepost.OriginalPost.Content
            )
        )).Returns(repost);
    }

    public void PostDoesNotExist(long postId)
    {
        A.CallTo(() => PublicationRepository.FindPostById(postId)).Returns(Task.FromResult<IPost?>(null));
    }

    public void PostExists(IPost post)
    {
        A.CallTo(() => PublicationRepository.FindPostById(post.Id)).Returns(post);
    }

    public void UserDoesNotExist(string username)
    {
        A.CallTo(() => UserRepository.FindByUsername(username)).Returns(Task.FromResult<IUser?>(null));
    }
    
    public void UserExists(IUser user)
    {
        A.CallTo(() => UserRepository.FindByUsername(user.Username)).Returns(user);
    }

    public void UserHasNotMadePublicationsToday(IUser user)
    {
        A.CallTo(() => DomainPersistenceAdapter.AmountOfPublicationsMadeTodayBy(user)).Returns((ushort)0);
    }

    public void UserHasReachedMaxAllowedDailyPublications(IUser user)
    {
        A.CallTo(() => DomainPersistenceAdapter.AmountOfPublicationsMadeTodayBy(user))
            .Returns(DomainConfig.MaxAllowedDailyPublicationsByUser);
    }
}
