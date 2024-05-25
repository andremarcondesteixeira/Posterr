using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Posterr.Core.Application.UseCases.CreateNewPost;
using Posterr.Core.Application.UseCases.CreateNewRepost;
using Posterr.Core.Application.UseCases.PaginatePublications;
using Posterr.Core.Boundaries.Configuration;
using Posterr.Presentation.Web.RestApi.Controllers.Models;

namespace Posterr.Presentation.Web.RestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PublicationsController(
    CreateNewPostUseCase createNewPostUseCase,
    CreateNewRepostUseCase createNewRepostUseCase,
    PaginatePublicationsUseCase paginatePublicationsUseCase,
    IDomainConfig domainConfig,
    LinkGenerator linkGenerator
) : ControllerBase
{
    [HttpPost]
    public Task<CreateNewPostResponseDTO> CreateNewPost([FromBody] CreateNewPostRequestDTO request) =>
        createNewPostUseCase.Run(request);

    [HttpPost]
    public Task<CreateNewRepostResponseDTO> CreateNewRepost([FromBody] CreateNewRepostRequestDTO request) =>
        createNewRepostUseCase.Run(request);

    [HttpGet]
    public async Task<PublicationsPaginationDTO> PaginatePublications([FromQuery] int pageNumber)
    {
        short pageSize = pageNumber == 1 ? domainConfig.Pagination.FirstPageSize
                                         : domainConfig.Pagination.NextPagesSize;
        var request = new PaginatePublicationsRequestDTO(pageNumber, pageSize);
        IList<PaginatePublicationsResponseItemDTO> publications = await paginatePublicationsUseCase.Run(request);

        IList<PublicationDTO> publicationDTOs = publications.Select(
            PublicationDTO.FromPaginatePublicationsResponseItemDTO
        ).ToList();

        var baseUrl = linkGenerator.GetUriByAction(HttpContext);
        return new PublicationsPaginationDTO(publicationDTOs, request.PageNumber, pageSize, baseUrl!);
    }
}
