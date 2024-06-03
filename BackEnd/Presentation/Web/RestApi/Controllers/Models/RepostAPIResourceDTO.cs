using Posterr.Presentation.Web.RestApi.Controllers.HATEOAS.HAL;

namespace Posterr.Presentation.Web.RestApi.Controllers.Models;

public sealed record RepostAPIResourceDTO : APIResource<RepostAPIResourceDTO.EmbeddedObjects>
{
    public readonly bool IsRepost = true;
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

    public sealed record EmbeddedObjects(PostAPIResourceDTO OriginalPost, UserAPIResourceDTO Author);
}
