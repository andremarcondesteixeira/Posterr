using Posterr.Presentation.Web.RestApi.Controllers.HATEOAS.HAL;

namespace Posterr.Presentation.Web.RestApi.Controllers.Models;

public record UserAPIResourceDTO : APIResource
{
    public required long Id { get; init; }
    public required string Username { get; init; }

    public UserAPIResourceDTO(string listUsersEndpointUrl)
    {
        Links.Add("self", [new($"{listUsersEndpointUrl}/{Id}")]);
    }
}
