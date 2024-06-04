using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Posterr.Core.Application.UseCases.GetUserById;
using Posterr.Core.Application.UseCases.ListUsers;
using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Core.Shared.Exceptions;
using Posterr.Presentation.Web.RestApi.Controllers.SharedModels;

namespace Posterr.Presentation.Web.RestApi.Controllers.Users;

[ApiController]
[Route("api/[controller]")]
public class UsersController(
    ListUsersUseCase listUsersUseCase,
    GetUserByIdUseCase getUserByIdUseCase,
    LinkGenerationService linkGenerationService
) : ControllerBase
{
    [HttpGet(Name = nameof(ListUsers))]
    [ProducesResponseType<UsersListAPIResourceDTO>(StatusCodes.Status200OK)]
    public IActionResult ListUsers()
    {
        IList<IUser> users = listUsersUseCase.Run();
        IList<UserAPIResourceDTO> userResources = users
            .Select(user => UserAPIResourceDTO.FromIUser(user, linkGenerationService))
            .ToList();
        UsersListAPIResourceDTO response = new(linkGenerationService)
        {
            Embedded = new UsersListAPIResourceDTO.EmbeddedObjects(userResources)
        };
        return Ok(response);
    }

    [HttpGet("{userId}")]
    [ProducesResponseType<UserAPIResourceDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public IActionResult GetUserById([FromRoute] long userId)
    {
        try
        {
            IUser user = getUserByIdUseCase.Run(userId);
            var response = UserAPIResourceDTO.FromIUser(user, linkGenerationService);
            return Ok(response);
        }
        catch (PosterrException e)
        {
            return Problem(
                title: e.Message,
                detail: e.Mitigation,
                instance: linkGenerationService.Generate(
                    controller: nameof(UsersController),
                    action: nameof(GetUserById),
                    values: new { userId }
                ),
                statusCode: e switch
                {
                    UserNotFoundException => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError
                }
            );
        }
    }
}
