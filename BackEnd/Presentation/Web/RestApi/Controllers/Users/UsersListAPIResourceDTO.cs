using Microsoft.AspNetCore.Mvc;
using Posterr.Presentation.Web.RestApi.Controllers.SharedModels;
using Posterr.Presentation.Web.RestApi.Controllers.SharedModels.HATEOAS.HAL;

namespace Posterr.Presentation.Web.RestApi.Controllers.Users;

public sealed record UsersListAPIResourceDTO : APIResource<UsersListAPIResourceDTO.EmbeddedObjects>
{
    public int Count { get => Embedded.Users.Count; }

    public UsersListAPIResourceDTO(IUrlHelper urlHelper)
    {
        string selfUrl = urlHelper.ActionLink(
            nameof(UsersController.ListUsers),
            nameof(UsersController).Replace("Controller", "")
        )!;
        APIResourceLinkDTO selfResourceLink = new(selfUrl);
        Links.Add("self", [selfResourceLink]);
    }

    public sealed record EmbeddedObjects(IList<UserAPIResourceDTO> Users);
}
