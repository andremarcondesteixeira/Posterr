using Microsoft.AspNetCore.Mvc;
using Posterr.Core.Application.UseCases.CreateNewPost;
using Posterr.Core.Application.UseCases.PaginatePublications;

namespace Posterr.Presentation.API.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
public class PublicationsController(
    CreateNewPostUseCase createNewPostUseCase,
    PaginatePublicationsUseCase paginatePublicationsUseCase
) : ControllerBase
{
    [HttpPost]
    public Task<CreateNewPostResponse> CreateNewPost([FromBody] CreateNewPostRequest request) =>
        createNewPostUseCase.Run(request);

    [HttpGet]
    public Task<IList<PaginatePublicationsResponseItem>> ListPosts([FromBody] PaginatePublicationsRequest request) =>
        paginatePublicationsUseCase.Run(request);
}
