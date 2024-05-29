using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Posterr.Core.Application.UseCases.ListPublicationsWithPagination;
using Posterr.Core.Boundaries.Configuration;
using Posterr.Core.Shared.Exceptions;
using Posterr.Presentation.Web.RestApi.Controllers.Models;

namespace Posterr.Presentation.Web.RestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PublicationsController(ListPublicationsWithPaginationUseCase listPaginationsUseCase,
                                    IDomainConfig domainConfig,
                                    LinkGenerator linkGenerator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ListPublications([FromQuery] int pageNumber)
    {
        string baseUrl = linkGenerator.GetUriByAction(HttpContext)!;

        try
        {
            var paginationParameters = new PaginationParameters(pageNumber, domainConfig);
            IList<PublicationsPageEntryDTO> publications = await listPaginationsUseCase.Run(paginationParameters);

            IList<ListPublicationsResponseItemDTO> publicationDTOs = publications.Select(
                ListPublicationsResponseItemDTO.FromPublicationsPageEntryDTO
            ).ToList();

            return Ok(new ListPublicationsResponseDTO(publicationDTOs, paginationParameters, baseUrl));
        }
        catch (InvalidPageNumberException ex)
        {
            return Problem(
                title: ex.Message,
                statusCode: StatusCodes.Status400BadRequest,
                detail: ex.Mitigation,
                instance: $"{baseUrl}&pageNumber={pageNumber}"
            );
        }
    }
}
