using Microsoft.AspNetCore.Http;
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
    public async Task<IActionResult> PaginatePublications([FromQuery] int pageNumber)
    {
        string? baseUrl = linkGenerator.GetUriByAction(HttpContext);

        try
        {
            short pageSize = pageNumber == 1 ? domainConfig.Pagination.FirstPageSize
                                             : domainConfig.Pagination.NextPagesSize;
            var dto = new PaginatePublicationsRequestDTO(pageNumber, pageSize);
            IList<PaginatePublicationsResponseItemDTO> publications = await paginatePublicationsUseCase.Run(dto);

            IList<PublicationDTO> publicationDTOs = publications.Select(
                PublicationDTO.FromPaginatePublicationsResponseItemDTO
            ).ToList();

            return Ok(new PublicationsPaginationDTO(publicationDTOs, dto.PageNumber, pageSize, baseUrl!));
        }
        catch (InvalidPageNumberException ex)
        {
            return Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid page number",
                instance: $"{baseUrl}&pageNumber={pageNumber}"
            );
        }
    }
}
