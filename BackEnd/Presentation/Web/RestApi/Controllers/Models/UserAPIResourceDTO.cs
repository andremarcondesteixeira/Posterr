using Posterr.Presentation.Web.RestApi.Controllers.HATEOAS.HAL;

namespace Posterr.Presentation.Web.RestApi.Controllers.Models;

public record UserAPIResourceDTO : APIResource
{
    public long Id { get; }
    public required string Username { get; init; }

    public UserAPIResourceDTO(long id, string listUsersEndpointUrl)
    {
        Id = id;
        Links.Add("self", [new($"{listUsersEndpointUrl}/{Id}")]);
    }
}
