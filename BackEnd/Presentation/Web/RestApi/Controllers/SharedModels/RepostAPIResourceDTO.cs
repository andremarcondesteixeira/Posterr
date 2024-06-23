using Posterr.Core.Shared.EntitiesInterfaces;
using Posterr.Presentation.Web.RestApi.Controllers.Publications;
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

    public RepostAPIResourceDTO(long id, LinkGenerationService linkGenerationService)
    {
        string selfUrl = linkGenerationService.Generate(
            controller: nameof(PublicationsController),
            action: nameof(PublicationsController.GetPublicationById),
            values: new { publicationId = id }
        )!;
        APIResourceLinkDTO selfResourceLink = new(selfUrl);
        Id = id;
        Links.Add("self", [selfResourceLink]);
    }

    public static RepostAPIResourceDTO FromIRepost(IRepost repost, LinkGenerationService linkGenerationService)
    {
        var originalPost = PostAPIResourceDTO.FromIPost(repost.OriginalPost, linkGenerationService);
        UserAPIResourceDTO repostAuthor = new(repost.Author.Id, linkGenerationService)
        {
            Username = repost.Author.Username
        };

        return new RepostAPIResourceDTO(repost.Id, linkGenerationService)
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
