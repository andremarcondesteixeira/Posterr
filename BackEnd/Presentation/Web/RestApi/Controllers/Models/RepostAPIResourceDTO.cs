using Microsoft.AspNetCore.Mvc;
using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Presentation.Web.RestApi.Controllers.HATEOAS.HAL;
using Posterr.Presentation.Web.RestApi.Controllers.Users;

namespace Posterr.Presentation.Web.RestApi.Controllers.Models;

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

    public RepostAPIResourceDTO(long id, string listPublicationsEndpointUrl)
    {
        Id = id;
        Links.Add("self", [new($"{listPublicationsEndpointUrl}/{Id}")]);
    }

    public static RepostAPIResourceDTO FromIRepost(IRepost repost, IUrlHelper Url)
    {
        var originalPost = PostAPIResourceDTO.FromIPost(repost.OriginalPost, Url);

        string listUsersUrl = Url.ActionLink(
            nameof(UsersController.ListUsers),
            nameof(UsersController).Replace("Controller", "")
        )!;
        var repostAuthor = new UserAPIResourceDTO(repost.Author.Id, listUsersUrl)
        {
            Username = repost.Author.Username,
        };

        string listPublicationsUrl = Url.ActionLink(
            nameof(PublicationsController.ListPublications),
            nameof(PublicationsController).Replace("Controller", "")
        )!;
        return new RepostAPIResourceDTO(repost.Id, listPublicationsUrl)
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
