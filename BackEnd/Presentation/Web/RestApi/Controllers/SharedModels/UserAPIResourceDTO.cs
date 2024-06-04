using Microsoft.AspNetCore.Mvc;
using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Presentation.Web.RestApi.Controllers.SharedModels.HATEOAS.HAL;
using Posterr.Presentation.Web.RestApi.Controllers.Users;

namespace Posterr.Presentation.Web.RestApi.Controllers.SharedModels;

public record UserAPIResourceDTO : APIResource
{
    public long Id { get; }
    public required string Username { get; init; }

    public UserAPIResourceDTO(long id, IUrlHelper urlHelper)
    {
        string selfUrl = urlHelper.ActionLink(
            nameof(UsersController.GetUserById),
            nameof(UsersController).Replace("Controller", ""),
            id
        )!;
        Id = id;
        Links.Add("self", [new(selfUrl)]);
    }

    public static UserAPIResourceDTO FromIUser(IUser user, IUrlHelper urlHelper)
    {
        return new UserAPIResourceDTO(user.Id, urlHelper)
        {
            Username = user.Username
        };
    }
}
