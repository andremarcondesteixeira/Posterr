using Posterr.Core.Shared.EntitiesInterfaces;
using Posterr.Presentation.Web.RestApi.Controllers.SharedModels.HATEOAS.HAL;
using Posterr.Presentation.Web.RestApi.Controllers.Users;

namespace Posterr.Presentation.Web.RestApi.Controllers.SharedModels;

public record UserAPIResourceDTO : APIResource
{
    public long Id { get; }
    public required string Username { get; init; }

    public UserAPIResourceDTO(long id, LinkGenerationService linkGenerationService)
    {
        string selfUrl = linkGenerationService.Generate(
            controller: nameof(UsersController).Replace("Controller", ""),
            action: nameof(UsersController.GetUserById),
            values: new { userId = id }
        )!;
        Id = id;
        Links.Add("self", [new(selfUrl)]);
    }

    public static UserAPIResourceDTO FromIUser(IUser user, LinkGenerationService linkGenerationService)
    {
        return new UserAPIResourceDTO(user.Id, linkGenerationService)
        {
            Username = user.Username
        };
    }
}
