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
) : IUseCase<CreateNewRepostUseCaseInputDTO, CreateNewRepostUseCaseOutputDTO>
{
    private readonly IUsersRepository _userRepository = _userRepository;
    private readonly IPublicationsRepository _publicationRepository = _publicationRepository;
    private readonly IDomainPersistencePort _domainPersistenceAdapter = _domainPersistenceAdapter;
    private readonly IDomainConfig _domainConfig = _domainConfig;

    public async Task<CreateNewRepostUseCaseOutputDTO> Run(CreateNewRepostUseCaseInputDTO input)
    {
        IUser user = await _userRepository.FindByUsername(input.AuthorUsername)
            ?? throw new UserNotFoundException(input.AuthorUsername);

        IPost originalPost = await _publicationRepository.FindPostById(input.OriginalPostId)
            ?? throw new PostNotFoundException(input.OriginalPostId);

        var unpublishedRepost = new UnpublishedRepost(user, originalPost, _domainConfig);
        var publishedRepost = await unpublishedRepost.Publish(_domainPersistenceAdapter);

        return new CreateNewRepostUseCaseOutputDTO()
        {
            AuthorUsername = publishedRepost.Author.Username,
            PublicationDate = publishedRepost.PublicationDate,
            OriginalPost = new CreateNewRepostUseCaseOutputDTO.OriginalPostData()
            {
                Id = publishedRepost.OriginalPost.Id,
                AuthorUsername = publishedRepost.OriginalPost.Author.Username,
                PublicationDate = publishedRepost.OriginalPost.PublicationDate,
                Content = publishedRepost.OriginalPost.Content
            },
        };
    }
}
