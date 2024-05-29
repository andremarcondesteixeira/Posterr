using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Posterr.Core.Application.UseCases.CreateNewPost;
using Posterr.Core.Shared.Exceptions;

namespace Posterr.Presentation.Web.RestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController(
    CreateNewPostUseCase createNewPostUseCase,
    LinkGenerator linkGenerator
) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateNewPost([FromBody] CreateNewPostRequestDTO request)
    {
        string baseUrl = linkGenerator.GetUriByAction(HttpContext)!;

        try
        {
            var response = await createNewPostUseCase.Run(request);
            return Ok(response);
        }
        catch (PosterrException e)
        {
            return Problem(
                title: e.Message,
                detail: e.Mitigation,
                instance: baseUrl,
                statusCode: e switch
                {
                    UserNotFoundException => StatusCodes.Status404NotFound,
                    EmptyPostContentException or MaxPostContentLengthExceededException => StatusCodes.Status400BadRequest,
                    MaxAllowedDailyPublicationsByUserExceededException => StatusCodes.Status403Forbidden,
                    _ => StatusCodes.Status500InternalServerError
                }
            );
        }
    }
}
