using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Posterr.Core.Application.UseCases.CreateNewPost;
using Posterr.Core.Application.UseCases.CreateNewRepost;
using Posterr.Core.Application.UseCases.GetPublicationById;
using Posterr.Core.Application.UseCases.ListPublications;
using Posterr.Core.Application.UseCases.SearchPublications;
using Posterr.Core.Shared.ConfigurationInterfaces;
using Posterr.Core.Shared.EntitiesInterfaces;
using Posterr.Core.Shared.Enums;
using Posterr.Core.Shared.Exceptions;
using Posterr.Presentation.Web.RestApi.Controllers.SharedModels;
using static Posterr.Presentation.Web.RestApi.Controllers.Publications.PublicationsListAPIResourceDTO;

namespace Posterr.Presentation.Web.RestApi.Controllers.Publications;

[ApiController]
[Route("api/[controller]")]
public class PublicationsController(
    IDomainConfig domainConfig,
    ListPublicationsUseCase listPublicationsUseCase,
    GetPublicationByIdUseCase getPublicationByIdUseCase,
    CreateNewPostUseCase createNewPostUseCase,
    CreateNewRepostUseCase createNewRepostUseCase,
    SearchPublicationsUseCase searchPublicationsUseCase,
    LinkGenerationService linkGenerationService
) : ControllerBase
{
    [HttpGet(Name = nameof(ListPublications))]
    [ProducesResponseType<PublicationsListAPIResourceDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public IActionResult ListPublications([FromQuery] long lastSeenPublicationId, [FromQuery] bool isFirstPage, [FromQuery] int sortingOrder)
    {
        try
        {
            var paginationParameters = new ListPublicationsUseCaseInputDTO(isFirstPage, lastSeenPublicationId, domainConfig, sortingOrder);
            IList<IPublication> useCaseOutput = listPublicationsUseCase.Run(paginationParameters);
            var publications = useCaseOutput.Select(p => PublicationAPIResourceDTO.FromIPublication(p, linkGenerationService)).ToList();
            var response = new PublicationsListAPIResourceDTO(publications, paginationParameters, linkGenerationService)
            {
                Embedded = new EmbeddedObjects
                {
                    Publications = publications,
                }
            };

            return Ok(response);
        }
        catch (PosterrException ex)
        {
            return Problem(
                title: ex.Message,
                statusCode: ex switch
                {
                    InvalidSortOrderException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError,
                },
                detail: ex.Mitigation,
                instance: linkGenerationService.Generate(
                    controller: nameof(PublicationsController),
                    action: nameof(ListPublications),
                    values: new { lastSeenPublicationId, isFirstPage }
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
            var response = PostAPIResourceDTO.FromIPost(useCaseOutput, linkGenerationService);
            return Ok(response);
        }
        catch (PosterrException e)
        {
            return Problem(
                title: e.Message,
                detail: e.Mitigation,
                instance: linkGenerationService.Generate(
                    controller: nameof(PublicationsController),
                    action: nameof(CreateNewPost),
                    values: null
                ),
                statusCode: e switch
                {
                    UserNotFoundException => StatusCodes.Status404NotFound,
                    EmptyPostContentException or MaxPublicationContentLengthExceededException => StatusCodes.Status400BadRequest,
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
                return Ok(PostAPIResourceDTO.FromIPost(post, linkGenerationService));
            }

            return Ok(RepostAPIResourceDTO.FromIRepost((IRepost)publication, linkGenerationService));
        }
        catch (PosterrException e)
        {
            return Problem(
                title: e.Message,
                detail: e.Mitigation,
                instance: linkGenerationService.Generate(
                    controller: nameof(PublicationsController),
                    action: nameof(GetPublicationById),
                    values: new { publicationId }
                ),
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
    public IActionResult CreateNewRepost([FromRoute] long publicationId, [FromBody] CreateNewRepostRequestBodyDTO requestBody)
    {
        try
        {
            CreateNewRepostUseCaseInputDTO useCaseInput = new(requestBody.AuthorUsername, requestBody.Content, publicationId);
            IRepost useCaseOutput = createNewRepostUseCase.Run(useCaseInput);
            var responseResource = RepostAPIResourceDTO.FromIRepost(useCaseOutput, linkGenerationService);
            return Ok(responseResource);
        }
        catch (PosterrException e)
        {
            return Problem(
                title: e.Message,
                detail: e.Mitigation,
                instance: linkGenerationService.Generate(
                    controller: nameof(PublicationsController),
                    action: nameof(CreateNewRepost),
                    values: null
                ),
                statusCode: e switch
                {
                    UserNotFoundException or PostNotFoundException => StatusCodes.Status404NotFound,
                    MaxAllowedDailyPublicationsByUserExceededException or DuplicatedRepostException => StatusCodes.Status403Forbidden,
                    _ => StatusCodes.Status500InternalServerError
                }
            );
        }
    }

    [HttpGet("search", Name = nameof(SearchPublications))]
    [ProducesResponseType<PublicationsListAPIResourceDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public IActionResult SearchPublications([FromQuery] string searchTerm)
    {
        try
        {
            SearchPublicationsUseCaseInputDTO input = new(searchTerm);
            IList<IPublication> useCaseOutput = searchPublicationsUseCase.Run(input);
            var publications = useCaseOutput.Select(p => PublicationAPIResourceDTO.FromIPublication(p, linkGenerationService)).ToList();
            SearchResultsAPIResourceDTO response = new(input, linkGenerationService)
            {
                Embedded = new()
                {
                    Publications = publications,
                }
            };
            return Ok(response);
        }
        catch(PosterrException e)
        {
            return Problem(
                title: e.Message,
                detail: e.Mitigation,
                instance: linkGenerationService.Generate(
                    controller: nameof(PublicationsController),
                    action: nameof(SearchPublications),
                    values: null
                ),
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }
}
