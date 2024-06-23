using Posterr.Core.Domain.Entities.Publications;
using Posterr.Core.Shared.ConfigurationInterfaces;
using Posterr.Core.Shared.EntitiesInterfaces;
using Posterr.Core.Shared.Exceptions;
using Posterr.Core.Shared.PersistenceInterfaces;

namespace Posterr.Core.Application.UseCases.CreateNewRepost;

public sealed class CreateNewRepostUseCase(
    IUsersRepository _userRepository,
    IPublicationsRepository _publicationsRepository,
    IDomainConfig _domainConfig
) : IUseCase<CreateNewRepostUseCaseInputDTO, IRepost>
{
    private readonly IUsersRepository _userRepository = _userRepository;
    private readonly IPublicationsRepository _publicationsRepository = _publicationsRepository;
    private readonly IDomainConfig _domainConfig = _domainConfig;

    public IRepost Run(CreateNewRepostUseCaseInputDTO input)
    {
        IUser user = _userRepository.FindByUsername(input.AuthorUsername)
            ?? throw new UserNotFoundException(input.AuthorUsername);

        IPublication originalPost = _publicationsRepository.FindById(input.OriginalPostId)
            ?? throw new PostNotFoundException(input.OriginalPostId);

        if (originalPost is IRepost)
        {
            throw new CannotRepostRepostException(input.AuthorUsername, input.OriginalPostId);
        }

        var unpublishedRepost = new UnpublishedRepost(user, input.Content, (IPost)originalPost, _domainConfig);

        bool isDuplicatedRepost = _publicationsRepository.CountRepostsByUserAndOriginalPost(user, (IPost)originalPost) > 0;

        if (isDuplicatedRepost)
        {
            throw new DuplicatedRepostException(unpublishedRepost);
        }

        return unpublishedRepost.Publish(_publicationsRepository);
    }
}
