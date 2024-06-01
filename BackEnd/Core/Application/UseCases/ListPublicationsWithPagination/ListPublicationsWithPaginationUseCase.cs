using Posterr.Core.Boundaries.Persistence;
using Posterr.Core.Boundaries.EntitiesInterfaces;

namespace Posterr.Core.Application.UseCases.ListPublicationsWithPagination;

public sealed class ListPublicationsWithPaginationUseCase(IPublicationsRepository repository) : IUseCase<PaginationParameters, IList<PublicationsPageEntryDTO>>
{
    public IList<PublicationsPageEntryDTO> Run(PaginationParameters paginationParameters)
    {
        var publications = repository.Paginate(paginationParameters.LastRowNumber, paginationParameters.PageSize);

        return publications.Select(publication =>
        {
            if (publication is IPost post)
            {
                return new PublicationsPageEntryDTO()
                {
                    IsRepost = false,
                    Post = new PublicationsPageEntryDTO.PostData(
                        post.Id,
                        post.Author.Username,
                        post.PublicationDate,
                        post.Content
                    )
                };
            }

            var repost = (IRepost)publication;

            return new PublicationsPageEntryDTO()
            {
                IsRepost = true,
                Post = new PublicationsPageEntryDTO.PostData(
                    repost.OriginalPost.Id,
                    repost.OriginalPost.Author.Username,
                    repost.OriginalPost.PublicationDate,
                    repost.OriginalPost.Content
                ),
                Repost = new PublicationsPageEntryDTO.RepostData(repost.Author.Username, repost.PublicationDate)
            };
        }).ToList();
    }
}
