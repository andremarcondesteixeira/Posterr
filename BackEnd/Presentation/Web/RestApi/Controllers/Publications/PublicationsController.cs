using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Posterr.Core.Application.UseCases.CreateNewPost;
using Posterr.Core.Application.UseCases.CreateNewRepost;
using Posterr.Core.Application.UseCases.GetPublicationById;
using Posterr.Core.Application.UseCases.ListPublicationsWithPagination;
using Posterr.Core.Boundaries.Configuration;
using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Core.Shared.Exceptions;
using Posterr.Presentation.Web.RestApi.Controllers.Publications;
using Posterr.Presentation.Web.RestApi.Controllers.SharedModels;
using static Posterr.Presentation.Web.RestApi.Controllers.Publications.PublicationsListAPIResourceDTO;

namespace Posterr.Presentation.Web.RestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PublicationsController(
    IDomainConfig domainConfig,
    ListPublicationsUseCase listPublicationsWithPaginationUseCase,
    GetPublicationByIdUseCase getPublicationByIdUseCase,
    CreateNewPostUseCase createNewPostUseCase,
    CreateNewRepostUseCase createNewRepostUseCase
) : ControllerBase
{
    [HttpGet(Name = nameof(ListPublications))]
    [ProducesResponseType<PublicationsListAPIResourceDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public IActionResult ListPublications([FromQuery] int pageNumber)
    {
        try
        {
            var paginationParameters = new ListPublicationsUseCaseInputDTO(pageNumber, domainConfig);
            IList<IPublication> useCaseOutput = listPublicationsWithPaginationUseCase.Run(paginationParameters);
            var publications = useCaseOutput.Select(p => PublicationAPIResourceDTO.FromIPublication(p, Url)).ToList();
            var response = new PublicationsListAPIResourceDTO(publications, paginationParameters, Url)
            {
                Embedded = new EmbeddedObjects
                {
                    Publications = publications,
                }
            };

            return Ok(response);
        }
        catch (InvalidPageNumberException ex)
        {
            return Problem(
                title: ex.Message,
                statusCode: StatusCodes.Status400BadRequest,
                detail: ex.Mitigation,
                instance: Url.ActionLink(
                    nameof(ListPublications),
                    nameof(PublicationsController).Replace("Controller", ""),
                    pageNumber
                )
            );
        }
    }

    [HttpPost]
    [ProducesResponseType<PostAPIResourceDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public IActionResult CreateNewPost([FromBody] CreateNewPostRequestBodyDTO requestBody)
    {
        try
        {
            CreateNewPostUseCaseInputDTO useCaseInput = new(requestBody.AuthorUsername, requestBody.Content);
            IPost useCaseOutput = createNewPostUseCase.Run(useCaseInput);
            var response = PostAPIResourceDTO.FromIPost(useCaseOutput, Url);
            return Ok(response);
        }
        catch (PosterrException e)
        {
            return Problem(
                title: e.Message,
                detail: e.Mitigation,
                instance: Url.ActionLink(nameof(CreateNewPost), nameof(PublicationsController).Replace("Controller", "")),
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

    [HttpGet("{publicationId}")]
    [ProducesResponseType<PostAPIResourceDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType<RepostAPIResourceDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public IActionResult GetPublicationById([FromRoute] long publicationId)
    {
        try
        {
            var publication = getPublicationByIdUseCase.Run(new(publicationId));

            if (publication is IPost post)
            {
                return Ok(PostAPIResourceDTO.FromIPost(post, Url));
            }

            return Ok(RepostAPIResourceDTO.FromIRepost((IRepost)publication, Url));
        }
        catch(PosterrException e)
        {
            return Problem(
                title: e.Message,
                detail: e.Mitigation,
                instance: Url.ActionLink(nameof(GetPublicationById), nameof(PublicationsController).Replace("Controller", ""), publicationId),
                statusCode: e switch
                {
                    PublicationNotFoundException => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError
                }
            );
        }
    }

    [HttpPatch("{publicationId}")]
    [ProducesResponseType<RepostAPIResourceDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public IActionResult CreateNewRepost(long publicationId, [FromBody] CreateNewRepostRequestBodyDTO requestBody)
    {
        try
        {
            CreateNewRepostUseCaseInputDTO useCaseInput = new(requestBody.AuthorUsername, publicationId);
            IRepost useCaseOutput = createNewRepostUseCase.Run(useCaseInput);
            var responseResource = RepostAPIResourceDTO.FromIRepost(useCaseOutput, Url);
            return Ok(responseResource);
        }
        catch (PosterrException e)
        {
            return Problem(
                title: e.Message,
                detail: e.Mitigation,
                instance: Url.ActionLink(nameof(ListPublications), nameof(PublicationsController).Replace("Controller", ""))!,
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
