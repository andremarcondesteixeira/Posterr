using Microsoft.AspNetCore.Mvc;
using Posterr.Core.Application.UseCases.CreateNewPost;
using Posterr.Core.Application.UseCases.CreateNewRepost;
using Posterr.Core.Application.UseCases.PaginatePublications;

namespace Posterr.Presentation.Web.RestApi.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
public class PublicationsController(
    CreateNewPostUseCase createNewPostUseCase,
    CreateNewRepostUseCase createNewRepostUseCase,
    PaginatePublicationsUseCase paginatePublicationsUseCase
) : ControllerBase
{
    [HttpPost]
    public Task<CreateNewPostResponse> CreateNewPost([FromBody] CreateNewPostRequest request) =>
        createNewPostUseCase.Run(request);

    [HttpPost]
    public Task<CreateNewRepostResponse> CreateNewRepost([FromBody] CreateNewRepostRequest request) =>
        createNewRepostUseCase.Run(request);

    [HttpGet]
    public Task<IList<PaginatePublicationsResponseItem>> ListPosts([FromBody] PaginatePublicationsRequest request) =>
        paginatePublicationsUseCase.Run(request);
}
