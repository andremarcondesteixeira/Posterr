using Microsoft.AspNetCore.Mvc;
using Posterr.Core.Application.UseCases.ListPublicationsWithPagination;
using Posterr.Presentation.Web.RestApi.Controllers.HATEOAS.HAL;
using Posterr.Presentation.Web.RestApi.Controllers.Models;

namespace Posterr.Presentation.Web.RestApi.Controllers.Publications;

public sealed record PublicationsListDTO : APIResource<PublicationsListDTO.EmbeddedObjects>
{
    public int Count { get; set; }

    public PublicationsListDTO(
        IList<PublicationAPIResourceDTO> publications,
        ListPublicationsUseCaseInputDTO paginationParameters,
        IUrlHelper urlHelper)
    {
        string baseUrl = urlHelper.ActionLink(
            nameof(PublicationsController.ListPublications),
            nameof(PublicationsController).Replace("Controller", "")
        )!;
        Count = publications.Count;
        Links.Add("self", [new($"{baseUrl}?pageNumber={paginationParameters.PageNumber}")]);

        if (publications.Count >= paginationParameters.PageSize)
        {
            Links.Add("next", [new($"{baseUrl}?pageNumber={paginationParameters.PageNumber + 1}")]);
        }
    }

    public sealed record EmbeddedObjects
    {
        public required List<PublicationAPIResourceDTO> Publications { get; init; }
    }
}
