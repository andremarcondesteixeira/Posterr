using Posterr.Core.Application.Exceptions;
using Posterr.Core.Application.Interfaces;
using Posterr.Core.Domain;
using Posterr.Core.Domain.Publications;
using Posterr.Core.Domain.Users;

namespace Posterr.Core.Application.CreateNewRepost;

public sealed class CreateNewRepostUseCase(
    IUserRepository _userRepository,
    IPublicationRepository _publicationRepository,
    IDomainPersistencePort _domainPersistenceAdapter,
    IDomainConfig _domainConfig
)
{
    private readonly IUserRepository _userRepository = _userRepository
        ?? throw new ArgumentNullException(nameof(_userRepository));

    private readonly IPublicationRepository _publicationRepository = _publicationRepository
        ?? throw new ArgumentNullException(nameof(_publicationRepository));

    private readonly IDomainPersistencePort _domainPersistenceAdapter = _domainPersistenceAdapter
        ?? throw new ArgumentNullException(nameof(_domainPersistenceAdapter));

    private readonly IDomainConfig _domainConfig = _domainConfig
        ?? throw new ArgumentNullException(nameof(_domainConfig));

    public async Task<CreateNewRepostResponse> Run(CreateNewRepostRequest request)
    {
        ArgumentNullException.ThrowIfNull(nameof(request));

        IUser user = await _userRepository.FindByUsername(request.AuthorUsername)
            ?? throw new UserNotFoundException(request.AuthorUsername);

        IPost originalPost = await _publicationRepository.FindPostById(request.OriginalPostId)
            ?? throw new PostNotFoundException(request.OriginalPostId);

        var unpublishedRepost = new UnpublishedRepost(user, originalPost, _domainConfig);
        var publishedRepost = await unpublishedRepost.Publish(_domainPersistenceAdapter);

        return new CreateNewRepostResponse()
        {
            RepostAuthorUsername = publishedRepost.Author.Username,
            RepostPublicationDate = publishedRepost.PublicationDate,
            OriginalPost = new CreateNewRepostResponse.Original()
            {
                Id = publishedRepost.OriginalPost.Id,
                AuthorUsername = publishedRepost.OriginalPost.Author.Username,
                PublicationDate = publishedRepost.OriginalPost.PublicationDate,
                Content = publishedRepost.OriginalPost.Content
            },
        };
    }
}
