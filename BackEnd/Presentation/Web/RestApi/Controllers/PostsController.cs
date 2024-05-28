using Microsoft.AspNetCore.Mvc;
using Posterr.Core.Application.UseCases.CreateNewPost;

namespace Posterr.Presentation.Web.RestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController(CreateNewPostUseCase createNewPostUseCase) : ControllerBase
{
    [HttpPost]
    public Task<CreateNewPostResponseDTO> CreateNewPost([FromBody] CreateNewPostRequestDTO request) =>
        createNewPostUseCase.Run(request);
}
