using Posterr.Core.Boundaries.Persistence;
using Posterr.Core.Boundaries.EntitiesInterfaces;

namespace Posterr.Core.Application.UseCases.PaginatePublications;

public sealed class PaginatePublicationsUseCase(IPublicationsRepository repository)
    : IUseCase<PaginatePublicationsRequest, IList<PaginatePublicationsResponseItem>>
{
    public async Task<IList<PaginatePublicationsResponseItem>> Run(PaginatePublicationsRequest request)
    {
        var publications = await repository.Paginate(request.LastSeenRow, request.PageSize);

        return publications.Select(p =>
        {
            if (p is IPost post)
            {
                return new PaginatePublicationsResponseItem()
                {
                    IsRepost = false,
                    Post = new PaginatePublicationsResponseItem.PostData(
                        post.Id,
                        post.Author.Username,
                        post.PublicationDate,
                        post.Content
                    )
                };
            }


            var repost = (IRepost)p;

            return new PaginatePublicationsResponseItem()
            {
                IsRepost = false,
                Post = new PaginatePublicationsResponseItem.PostData(
                    repost.OriginalPost.Id,
                    repost.OriginalPost.Author.Username,
                    repost.OriginalPost.PublicationDate,
                    repost.OriginalPost.Content
                ),
                Repost = new PaginatePublicationsResponseItem.RepostData(repost.Author.Username, repost.PublicationDate)
            };
        }).ToList();
    }
}
