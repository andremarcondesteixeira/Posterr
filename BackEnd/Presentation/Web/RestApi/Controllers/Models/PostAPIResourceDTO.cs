using Posterr.Presentation.Web.RestApi.Controllers.HATEOAS.HAL;

namespace Posterr.Presentation.Web.RestApi.Controllers.Models;

public sealed record PostAPIResourceDTO : APIResource<PostAPIResourceDTO.EmbeddedObjects>
{
    public readonly bool IsRepost = false;
    public long Id { get; }
    public required string AuthorUsername { get; init; }
    public required DateTime PublicationDate { get; init; }
    public required string Content { get; init; }

    public PostAPIResourceDTO(long id, string listPublicationsEndpointUrl)
    {
        Id = id;
        Links.Add("self", [new($"{listPublicationsEndpointUrl}/{Id}")]);
    }

    public sealed record EmbeddedObjects(UserAPIResourceDTO Author);
}
