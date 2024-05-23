using Posterr.Core.Boundaries.Persistence;
using Posterr.Core.Boundaries.EntitiesInterfaces;

namespace Posterr.Core.Application.UseCases.PaginatePublications;

public sealed class PaginatePublicationsUseCase(IPublicationsRepository repository)
    : IUseCase<PaginatePublicationsRequestDTO, IList<PaginatePublicationsResponseItemDTO>>
{
    public async Task<IList<PaginatePublicationsResponseItemDTO>> Run(PaginatePublicationsRequestDTO request)
    {
        int lastSeenRow = request.PageNumber switch {
            1 => 0,
            _ => request.PageNumber * 20 - 25
        };
        var publications = await repository.Paginate(lastSeenRow, request.PageSize);

        return publications.Select(publication =>
        {
            if (publication is IPost post)
            {
                return new PaginatePublicationsResponseItemDTO()
                {
                    IsRepost = false,
                    Post = new PaginatePublicationsResponseItemDTO.PostData(
                        post.Id,
                        post.Author.Username,
                        post.PublicationDate,
                        post.Content
                    )
                };
            }

            var repost = (IRepost)publication;

            return new PaginatePublicationsResponseItemDTO()
            {
                IsRepost = true,
                Post = new PaginatePublicationsResponseItemDTO.PostData(
                    repost.OriginalPost.Id,
                    repost.OriginalPost.Author.Username,
                    repost.OriginalPost.PublicationDate,
                    repost.OriginalPost.Content
                ),
                Repost = new PaginatePublicationsResponseItemDTO.RepostData(repost.Author.Username, repost.PublicationDate)
            };
        }).ToList();
    }
}
