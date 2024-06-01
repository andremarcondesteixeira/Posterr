using Posterr.Core.Boundaries.Persistence;
using Posterr.Core.Boundaries.Configuration;
using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Core.Domain.Entities;
using Posterr.Core.Domain.Entities.Publications;
using Posterr.Core.Shared.Exceptions;

namespace Posterr.Core.Application.UseCases.CreateNewPost;

public sealed class CreateNewPostUseCase(
    IUsersRepository userRepository,
    IPublicationsRepository publicationsRepository,
    IDomainConfig domainConfig
) : IUseCase<CreateNewPostUseCaseInputDTO, CreateNewPostUseCaseOutputDTO>
{
    public CreateNewPostUseCaseOutputDTO Run(CreateNewPostUseCaseInputDTO input)
    {
        IUser user = userRepository.FindByUsername(input.AuthorUsername)
            ?? throw new UserNotFoundException(input.AuthorUsername);

        var unpublishedPost = new UnpublishedPost(user, input.Content, domainConfig);
        var publishedPost = unpublishedPost.Publish(publicationsRepository);

        return new CreateNewPostUseCaseOutputDTO()
        {
            Id = publishedPost.Id,
            AuthorUsername = publishedPost.Author.Username,
            PublicationDate = publishedPost.PublicationDate,
            Content = publishedPost.Content
        };
    }
}
