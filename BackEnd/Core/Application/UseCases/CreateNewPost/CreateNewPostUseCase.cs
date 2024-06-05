﻿using Posterr.Core.Boundaries.ConfigurationInterface;
using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Core.Boundaries.Persistence;
using Posterr.Core.Domain.Entities.Publications;
using Posterr.Core.Shared.Exceptions;

namespace Posterr.Core.Application.UseCases.CreateNewPost;

public sealed class CreateNewPostUseCase(
    IUsersRepository userRepository,
    IPublicationsRepository publicationsRepository,
    IDomainConfig domainConfig
) : IUseCase<CreateNewPostUseCaseInputDTO, IPost>
{
    public IPost Run(CreateNewPostUseCaseInputDTO input)
    {
        IUser user = userRepository.FindByUsername(input.AuthorUsername)
            ?? throw new UserNotFoundException(input.AuthorUsername);

        var unpublishedPost = new UnpublishedPost(user, input.Content, domainConfig);
        return unpublishedPost.Publish(publicationsRepository);
    }
}
