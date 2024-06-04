using Microsoft.AspNetCore.Mvc;
using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Presentation.Web.RestApi.Controllers.SharedModels;
using Posterr.Presentation.Web.RestApi.Controllers.SharedModels.HATEOAS.HAL;
using System.Text.Json.Serialization;

namespace Posterr.Presentation.Web.RestApi.Controllers.Publications;

public sealed record PublicationAPIResourceDTO : APIResource<PublicationAPIResourceDTO.EmbeddedObjects>
{
    public required bool IsRepost { get; init; }
    public long Id { get; }
    public required string AuthorUsername { get; init; }
    public required DateTime PublicationDate { get; init; }
    public required string Content { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? OriginalPostAuthorUsername { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? OriginalPostPublicationDate { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? OriginalPostContent { get; init; }

    public PublicationAPIResourceDTO(long id, IUrlHelper urlHelper)
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

    public static PublicationAPIResourceDTO FromIPublication(IPublication publication, IUrlHelper urlHelper)
    {
        if (publication is IRepost repost)
        {
            return new PublicationAPIResourceDTO(publication.Id, urlHelper)
            {
                IsRepost = true,
                AuthorUsername = publication.Author.Username,
                PublicationDate = publication.PublicationDate,
                Content = publication.Content,
                OriginalPostAuthorUsername = repost.OriginalPost.Author.Username,
                OriginalPostPublicationDate = repost.OriginalPost.PublicationDate,
                OriginalPostContent = repost.OriginalPost.Content,
                Embedded = new EmbeddedObjects
                {
                    Author = new UserAPIResourceDTO(publication.Author.Id, urlHelper)
                    {
                        Username = publication.Author.Username
                    },
                    OriginalPost = PostAPIResourceDTO.FromIPost(repost.OriginalPost, urlHelper)
                }
            };
        }

        return new PublicationAPIResourceDTO(publication.Id, urlHelper)
        {
            IsRepost = false,
            AuthorUsername = publication.Author.Username,
            PublicationDate = publication.PublicationDate,
            Content = publication.Content,
            Embedded = new EmbeddedObjects
            {
                Author = new UserAPIResourceDTO(publication.Author.Id, urlHelper)
                {
                    Username = publication.Author.Username
                }
            },
        };
    }

    public sealed record EmbeddedObjects
    {
        public required UserAPIResourceDTO Author { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public PostAPIResourceDTO? OriginalPost { get; set; }
    }
}
