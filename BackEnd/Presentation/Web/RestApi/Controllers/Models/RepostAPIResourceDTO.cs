using Posterr.Presentation.Web.RestApi.Controllers.HATEOAS.HAL;

namespace Posterr.Presentation.Web.RestApi.Controllers.Models;

public sealed record RepostAPIResourceDTO : APIResource<RepostAPIResourceDTO.EmbeddedObjects>
{
    public required long Id { get; init; }
    public required DateTime PublicationDate { get; init; }
    public required string Content { get; init; }

    public RepostAPIResourceDTO(string listPublicationsEndpointUrl)
    {
        Links.Add("self", [new($"{listPublicationsEndpointUrl}/{Id}")]);
    }

    public sealed record EmbeddedObjects(PostAPIResourceDTO OriginalPost, UserAPIResourceDTO Author);
}
