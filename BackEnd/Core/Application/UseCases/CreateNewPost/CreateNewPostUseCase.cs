﻿using Posterr.Core.Boundaries.Persistence;
using Posterr.Core.Application.UseCases.Exceptions;
using Posterr.Core.Boundaries.Configuration;
using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Core.Domain.Entities;
using Posterr.Core.Domain.Entities.Publications;

namespace Posterr.Core.Application.UseCases.CreateNewPost;

public sealed class CreateNewPostUseCase(
    IUsersRepository userRepository,
    IDomainPersistencePort domainPersistenceAdapter,
    IDomainConfig domainConfig
) : IUseCase<CreateNewPostRequest, CreateNewPostResponse>
{
    public IUsersRepository UserRepository { get; } = userRepository
        ?? throw new ArgumentNullException(nameof(userRepository));

    public IDomainPersistencePort DomainPersistenceAdapter { get; } = domainPersistenceAdapter
        ?? throw new ArgumentNullException(nameof(domainPersistenceAdapter));

    public IDomainConfig DomainConfig { get; } = domainConfig
        ?? throw new ArgumentNullException(nameof(domainConfig));

    public async Task<CreateNewPostResponse> Run(CreateNewPostRequest request)
    {
        ArgumentNullException.ThrowIfNull(nameof(request));

        IUser user = await userRepository.FindByUsername(request.Username)
            ?? throw new UserNotFoundException(request.Username);

        var unpublishedPost = new UnpublishedPost(user, request.Content, DomainConfig);
        var publishedPost = await unpublishedPost.Publish(DomainPersistenceAdapter);

        return new CreateNewPostResponse()
        {
            PostId = publishedPost.Id,
            AuthorUsername = publishedPost.Author.Username,
            PublicationDate = publishedPost.PublicationDate,
            PostContent = publishedPost.Content
        };
    }
}