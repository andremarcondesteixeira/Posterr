using Posterr.Core.Application.Exceptions;
using Posterr.Core.Application.Interfaces;
using Posterr.Core.Domain;
using Posterr.Core.Domain.Publications;
using Posterr.Core.Domain.Users;

namespace Posterr.Core.Application.CreateNewPost;

public sealed class CreateNewPostUseCase(
    IUserRepository userRepository,
    IDomainPersistencePort domainPersistenceAdapter,
    IDomainConfig domainConfig
)
{
    public IUserRepository UserRepository { get; } = userRepository
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
