using Posterr.Core.Application.Boundaries.Persistence;
using Posterr.Core.Application.Exceptions;
using Posterr.Core.Domain;
using Posterr.Core.Domain.Boundaries.Configuration;
using Posterr.Core.Domain.Boundaries.Persistence;
using Posterr.Core.Domain.Publications;

namespace Posterr.Core.Application.UseCases.CreateNewRepost;

public sealed class CreateNewRepostUseCase(
    IUsersRepository _userRepository,
    IPublicationsRepository _publicationRepository,
    IDomainPersistencePort _domainPersistenceAdapter,
    IDomainConfig _domainConfig
) : IUseCase<CreateNewRepostRequest, CreateNewRepostResponse>
{
    private readonly IUsersRepository _userRepository = _userRepository
        ?? throw new ArgumentNullException(nameof(_userRepository));

    private readonly IPublicationsRepository _publicationRepository = _publicationRepository
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
