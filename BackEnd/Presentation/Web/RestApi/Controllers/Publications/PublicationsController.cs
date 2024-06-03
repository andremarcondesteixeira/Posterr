using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Posterr.Core.Application.UseCases.CreateNewPost;
using Posterr.Core.Application.UseCases.CreateNewRepost;
using Posterr.Core.Application.UseCases.ListPublicationsWithPagination;
using Posterr.Core.Boundaries.Configuration;
using Posterr.Core.Boundaries.EntitiesInterfaces;
using Posterr.Core.Shared.Exceptions;
using Posterr.Presentation.Web.RestApi.Controllers.Models;
using Posterr.Presentation.Web.RestApi.Controllers.Publications;
using Posterr.Presentation.Web.RestApi.Controllers.Users;

namespace Posterr.Presentation.Web.RestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PublicationsController(
    IDomainConfig domainConfig,
    LinkGenerator linkGenerator,
    ListPublicationsUseCase listPublicationsWithPaginationUseCase,
    CreateNewPostUseCase createNewPostUseCase,
    CreateNewRepostUseCase createNewRepostUseCase
) : ControllerBase
{
    [HttpGet(Name = nameof(ListPublications))]
    public IActionResult ListPublications([FromQuery] int pageNumber)
    {
        string baseUrl = linkGenerator.GetUriByName(HttpContext, nameof(ListPublications))!;

        try
        {
            var paginationParameters = new ListPublicationsUseCaseInputDTO(pageNumber, domainConfig);
            IList<IPublication> useCaseOutput = listPublicationsWithPaginationUseCase.Run(paginationParameters);
            
            var publications = useCaseOutput.Select(publication => new PublicationsListDTO.PublicationsListItemDTO
            {
                Id = publication.Id,
                Author = publication.Author,
                PublicationDate = publication.PublicationDate,
                Content = publication.Content,
                OriginalPost = publication is IRepost repost ? repost.OriginalPost : null,
            }).ToList();

            var embeddedResponseObject = new PublicationsListDTO.PublicationsList
            {
                Publications = publications,
            };

            var response = new PublicationsListDTO(publications, paginationParameters, baseUrl)
            {
                Embedded = embeddedResponseObject
            };

            return Ok(response);
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
        string baseUrl = linkGenerator.GetUriByName(HttpContext, nameof(ListPublications))!;
        string listUsersUrl = linkGenerator.GetUriByAction(HttpContext, nameof(UsersController.ListUsers), nameof(UsersController))!;

        try
        {
            CreateNewPostUseCaseInputDTO useCaseInput = new(requestBody.AuthorUsername, requestBody.Content);
            IPost useCaseOutput = createNewPostUseCase.Run(useCaseInput);
            
            var author = new UserAPIResourceDTO(listUsersUrl)
            {
                Id = useCaseOutput.Author.Id,
                Username = useCaseOutput.Author.Username,
            };
            
            var response = new PostAPIResourceDTO(baseUrl)
            {
                Id = useCaseOutput.Id,
                PublicationDate = useCaseOutput.PublicationDate,
                Content = useCaseOutput.Content,
                Embedded = new PostAPIResourceDTO.EmbeddedObjects(author),
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
        string listUsersUrl = linkGenerator.GetUriByAction(HttpContext, nameof(UsersController.ListUsers), nameof(UsersController))!;

        try
        {
            CreateNewRepostUseCaseInputDTO useCaseInput = new(requestBody.AuthorUsername, publicationId);
            IRepost useCaseOutput = createNewRepostUseCase.Run(useCaseInput);

            var originalPostAuthor = new UserAPIResourceDTO(listUsersUrl)
            {
                Id = useCaseOutput.OriginalPost.Author.Id,
                Username = useCaseOutput.OriginalPost.Author.Username,
            };

            var originalPost = new PostAPIResourceDTO(baseUrl)
            {
                Id = useCaseOutput.OriginalPost.Id,
                PublicationDate = useCaseOutput.OriginalPost.PublicationDate,
                Content = useCaseOutput.OriginalPost.Content,
                Embedded = new PostAPIResourceDTO.EmbeddedObjects(originalPostAuthor),
            };

            var repostAuthor = new UserAPIResourceDTO(listUsersUrl)
            {
                Id = useCaseOutput.Author.Id,
                Username = useCaseOutput.Author.Username,
            };

            var response = new RepostAPIResourceDTO(baseUrl)
            {
                Id = useCaseOutput.Id,
                PublicationDate = useCaseOutput.PublicationDate,
                Content = useCaseOutput.Content,
                Embedded = new RepostAPIResourceDTO.EmbeddedObjects(originalPost, repostAuthor),
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
