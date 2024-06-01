using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Posterr.Core.Application.UseCases.CreateNewPost;
using Posterr.Core.Application.UseCases.CreateNewRepost;
using Posterr.Core.Application.UseCases.ListPublicationsWithPagination;
using Posterr.Core.Boundaries.Configuration;
using Posterr.Core.Shared.Exceptions;
using Posterr.Presentation.Web.RestApi.Controllers.Publications;

namespace Posterr.Presentation.Web.RestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PublicationsController(
    IDomainConfig domainConfig,
    LinkGenerator linkGenerator,
    ListPublicationsWithPaginationUseCase listPublicationsWithPaginationUseCase,
    CreateNewPostUseCase createNewPostUseCase,
    CreateNewRepostUseCase createNewRepostUseCase
) : ControllerBase
{
    [HttpGet]
    public IActionResult ListPublications([FromQuery] int pageNumber)
    {
        string baseUrl = linkGenerator.GetUriByAction(HttpContext)!;

        try
        {
            var paginationParameters = new PaginationParameters(pageNumber, domainConfig);
            IList<PublicationsPageEntryDTO> publications = listPublicationsWithPaginationUseCase.Run(paginationParameters);

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

    [HttpPost]
    public IActionResult CreateNewPost([FromBody] CreateNewPostRequestBodyDTO requestBody)
    {
        string baseUrl = linkGenerator.GetUriByAction(HttpContext)!;

        try
        {
            CreateNewPostUseCaseInputDTO useCaseInput = new(requestBody.AuthorUsername, requestBody.Content);
            CreateNewPostUseCaseOutputDTO useCaseOutput = createNewPostUseCase.Run(useCaseInput);
            CreateNewPostRequestResponseDTO response = new(baseUrl)
            {
                AuthorUsername = useCaseOutput.AuthorUsername,
                Content = useCaseOutput.Content,
                Id = useCaseOutput.Id,
                PublicationDate = useCaseOutput.PublicationDate,
            };
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

    [HttpPost("{publicationId}/repost")]
    public IActionResult CreateNewRepost(long publicationId, [FromBody] CreateNewRepostRequestBodyDTO requestBody)
    {
        string baseUrl = linkGenerator.GetUriByAction(HttpContext)!;

        try
        {
            CreateNewRepostUseCaseInputDTO useCaseInput = new(requestBody.AuthorUsername, publicationId);
            CreateNewRepostUseCaseOutputDTO useCaseOutput = createNewRepostUseCase.Run(useCaseInput);
            CreateNewRepostRequestResponseDTO.OriginalPostData originalPostData = new()
            {
                AuthorUsername = useCaseOutput.OriginalPost.AuthorUsername,
                Content = useCaseOutput.OriginalPost.Content,
                Id = useCaseOutput.OriginalPost.Id,
                PublicationDate = useCaseOutput.OriginalPost.PublicationDate,
            };
            CreateNewRepostRequestResponseDTO response = new(originalPostData, baseUrl)
            {
                AuthorUsername = useCaseOutput.AuthorUsername,
                PublicationDate = useCaseOutput.PublicationDate,
            };
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
                    MaxAllowedDailyPublicationsByUserExceededException or DuplicatedRepostException => StatusCodes.Status403Forbidden,
                    _ => StatusCodes.Status500InternalServerError
                }
            );
        }
    }
}
