using Posterr.Core.Boundaries.Configuration;
using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Core.Boundaries.Persistence;
using Posterr.Core.Domain.Entities.Publications;
using Posterr.Core.Shared.Exceptions;

namespace Posterr.Core.Application.UseCases.CreateNewRepost;

public sealed class CreateNewRepostUseCase(
    IUsersRepository _userRepository,
    IPublicationsRepository _publicationsRepository,
    IDomainConfig _domainConfig
) : IUseCase<CreateNewRepostUseCaseInputDTO, CreateNewRepostUseCaseOutputDTO>
{
    private readonly IUsersRepository _userRepository = _userRepository;
    private readonly IPublicationsRepository _publicationsRepository = _publicationsRepository;
    private readonly IDomainConfig _domainConfig = _domainConfig;

    public CreateNewRepostUseCaseOutputDTO Run(CreateNewRepostUseCaseInputDTO input)
    {
        IUser user = _userRepository.FindByUsername(input.AuthorUsername)
            ?? throw new UserNotFoundException(input.AuthorUsername);

        IPublication originalPost = _publicationsRepository.FindById(input.OriginalPostId)
            ?? throw new PostNotFoundException(input.OriginalPostId);

        if (originalPost is IRepost)
        {
            throw new CannotRepostRepostException(input.AuthorUsername, input.OriginalPostId);
        }

        var unpublishedRepost = new UnpublishedRepost(user, (IPost) originalPost, _domainConfig);
        var publishedRepost = unpublishedRepost.Publish(_publicationsRepository);

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
