using Posterr.Presentation.Web.RestApi.Controllers.SharedModels;
using Posterr.Presentation.Web.RestApi.Controllers.SharedModels.HATEOAS.HAL;

namespace Posterr.Presentation.Web.RestApi.Controllers.Users;

public sealed record UsersListAPIResourceDTO : APIResource<UsersListAPIResourceDTO.EmbeddedObjects>
{
    public int Count { get => Embedded.Users.Count; }

    public UsersListAPIResourceDTO(LinkGenerationService linkGenerationService)
    {
        string selfUrl = linkGenerationService.Generate(
            controller: nameof(UsersController),
            action: nameof(UsersController.ListUsers),
            values: null
        )!;
        APIResourceLinkDTO selfResourceLink = new(selfUrl);
        Links.Add("self", [selfResourceLink]);
    }

    public sealed record EmbeddedObjects(IList<UserAPIResourceDTO> Users);
}
