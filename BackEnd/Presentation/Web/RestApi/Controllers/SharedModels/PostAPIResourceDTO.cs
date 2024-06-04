using Microsoft.AspNetCore.Mvc;
using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Presentation.Web.RestApi.Controllers.SharedModels.HATEOAS.HAL;

namespace Posterr.Presentation.Web.RestApi.Controllers.SharedModels;

public sealed record PostAPIResourceDTO : APIResource<PostAPIResourceDTO.EmbeddedObjects>
{
    public bool IsRepost { get; } = false;
    public long Id { get; }
    public required string AuthorUsername { get; init; }
    public required DateTime PublicationDate { get; init; }
    public required string Content { get; init; }

    public PostAPIResourceDTO(long id, IUrlHelper urlHelper)
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

    public static PostAPIResourceDTO FromIPost(IPost post, IUrlHelper urlHelper)
    {
        UserAPIResourceDTO author = new(post.Author.Id, urlHelper)
        {
            Username = post.Author.Username
        };

        return new PostAPIResourceDTO(post.Id, urlHelper)
        {
            AuthorUsername = author.Username,
            PublicationDate = post.PublicationDate,
            Content = post.Content,
            Embedded = new EmbeddedObjects(author)
        };
    }

    public sealed record EmbeddedObjects(UserAPIResourceDTO Author);
}
