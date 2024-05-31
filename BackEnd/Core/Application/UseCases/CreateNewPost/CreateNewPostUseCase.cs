using Posterr.Core.Boundaries.Persistence;
using Posterr.Core.Boundaries.Configuration;
using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Core.Domain.Entities;
using Posterr.Core.Domain.Entities.Publications;
using Posterr.Core.Shared.Exceptions;

namespace Posterr.Core.Application.UseCases.CreateNewPost;

public sealed class CreateNewPostUseCase(
    IUsersRepository userRepository,
    IDomainPersistencePort domainPersistenceAdapter,
    IDomainConfig domainConfig
) : IUseCase<CreateNewPostUseCaseInputDTO, CreateNewPostUseCaseOutputDTO>
{
    public async Task<CreateNewPostUseCaseOutputDTO> Run(CreateNewPostUseCaseInputDTO input)
    {
        IUser user = await userRepository.FindByUsername(input.AuthorUsername)
            ?? throw new UserNotFoundException(input.AuthorUsername);

        var unpublishedPost = new UnpublishedPost(user, input.Content, domainConfig);
        var publishedPost = await unpublishedPost.Publish(domainPersistenceAdapter);

        return new CreateNewPostUseCaseOutputDTO()
        {
            Id = publishedPost.Id,
            AuthorUsername = publishedPost.Author.Username,
            PublicationDate = publishedPost.PublicationDate,
            Content = publishedPost.Content
        };
    }
}
