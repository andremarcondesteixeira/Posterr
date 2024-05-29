using Posterr.Core.Boundaries.Configuration;
using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Core.Boundaries.Persistence;
using Posterr.Core.Domain.Entities;
using Posterr.Core.Domain.Entities.Publications;
using Posterr.Core.Shared.Exceptions;

namespace Posterr.Core.Application.UseCases.CreateNewRepost;

public sealed class CreateNewRepostUseCase(
    IUsersRepository _userRepository,
    IPublicationsRepository _publicationRepository,
    IDomainPersistencePort _domainPersistenceAdapter,
    IDomainConfig _domainConfig
) : IUseCase<CreateNewRepostRequestDTO, CreateNewRepostResponseDTO>
{
    private readonly IUsersRepository _userRepository = _userRepository;
    private readonly IPublicationsRepository _publicationRepository = _publicationRepository;
    private readonly IDomainPersistencePort _domainPersistenceAdapter = _domainPersistenceAdapter;
    private readonly IDomainConfig _domainConfig = _domainConfig;

    public async Task<CreateNewRepostResponseDTO> Run(CreateNewRepostRequestDTO request)
    {
        IUser user = await _userRepository.FindByUsername(request.AuthorUsername)
            ?? throw new UserNotFoundException(request.AuthorUsername);

        IPost originalPost = await _publicationRepository.FindPostById(request.OriginalPostId)
            ?? throw new PostNotFoundException(request.OriginalPostId);

        var unpublishedRepost = new UnpublishedRepost(user, originalPost, _domainConfig);
        var publishedRepost = await unpublishedRepost.Publish(_domainPersistenceAdapter);

        return new CreateNewRepostResponseDTO()
        {
            RepostAuthorUsername = publishedRepost.Author.Username,
            RepostPublicationDate = publishedRepost.PublicationDate,
            OriginalPost = new CreateNewRepostResponseDTO.OriginalPostData()
            {
                Id = publishedRepost.OriginalPost.Id,
                AuthorUsername = publishedRepost.OriginalPost.Author.Username,
                PublicationDate = publishedRepost.OriginalPost.PublicationDate,
                Content = publishedRepost.OriginalPost.Content
            },
        };
    }
}
