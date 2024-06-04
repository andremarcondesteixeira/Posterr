using Microsoft.AspNetCore.Mvc;
using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Presentation.Web.RestApi.Controllers.SharedModels.HATEOAS.HAL;

namespace Posterr.Presentation.Web.RestApi.Controllers.SharedModels;

public sealed record RepostAPIResourceDTO : APIResource<RepostAPIResourceDTO.EmbeddedObjects>
{
    public bool IsRepost { get; } = true;
    public long Id { get; }
    public required DateTime PublicationDate { get; init; }
    public required string AuthorUsername { get; init; }
    public required string Content { get; init; }
    public required string OriginalPostAuthorUsername { get; init; }
    public required DateTime OriginalPostPublicationDate { get; init; }
    public required string OriginalPostContent { get; init; }

    public RepostAPIResourceDTO(long id, IUrlHelper urlHelper)
    {
        string selfUrl = urlHelper.ActionLink(
            nameof(PublicationsController.GetPublicationById),
            nameof(PublicationsController).Replace("Controller", ""),
            id
        )!;
        APIResourceLinkDTO selfResourceLink = new(selfUrl);
        Id = id;
        Links.Add("self", [selfResourceLink]);
    }

    public static RepostAPIResourceDTO FromIRepost(IRepost repost, IUrlHelper urlHelper)
    {
        var originalPost = PostAPIResourceDTO.FromIPost(repost.OriginalPost, urlHelper);
        UserAPIResourceDTO repostAuthor = new(repost.Author.Id, urlHelper)
        {
            Username = repost.Author.Username
        };

        return new RepostAPIResourceDTO(repost.Id, urlHelper)
        {
            AuthorUsername = repost.Author.Username,
            PublicationDate = repost.PublicationDate,
            Content = repost.Content,
            OriginalPostAuthorUsername = repost.OriginalPost.Author.Username,
            OriginalPostPublicationDate = repost.OriginalPost.PublicationDate,
            OriginalPostContent = repost.OriginalPost.Content,
            Embedded = new EmbeddedObjects(originalPost, repostAuthor),
        };
    }

    public sealed record EmbeddedObjects(PostAPIResourceDTO OriginalPost, UserAPIResourceDTO Author);
}
