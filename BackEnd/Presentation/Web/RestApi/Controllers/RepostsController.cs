using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Posterr.Core.Application.UseCases.CreateNewRepost;
using Posterr.Core.Shared.Exceptions;

namespace Posterr.Presentation.Web.RestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RepostsController(
    CreateNewRepostUseCase createNewRepostUseCase,
    LinkGenerator linkGenerator
) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateNewRepost([FromBody] CreateNewRepostRequestDTO request)
    {
        string baseUrl = linkGenerator.GetUriByAction(HttpContext)!;

        try
        {
            var response = await createNewRepostUseCase.Run(request);
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
                    UserNotFoundException or PostNotFoundException => StatusCodes.Status404NotFound,
                    MaxAllowedDailyPublicationsByUserExceededException => StatusCodes.Status403Forbidden,
                    _ => StatusCodes.Status500InternalServerError
                }
            );
        }
    }
}
