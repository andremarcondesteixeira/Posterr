using Microsoft.AspNetCore.Mvc;
using Posterr.Core.Application.UseCases.ListPublicationsWithPagination;
using Posterr.Presentation.Web.RestApi.Controllers.SharedModels.HATEOAS.HAL;

namespace Posterr.Presentation.Web.RestApi.Controllers.Publications;

public sealed record PublicationsListAPIResourceDTO : APIResource<PublicationsListAPIResourceDTO.EmbeddedObjects>
{
    public int Count { get => Embedded.Publications.Count; }

    public PublicationsListAPIResourceDTO(
        IList<PublicationAPIResourceDTO> publications,
        ListPublicationsUseCaseInputDTO paginationParameters,
        LinkGenerationService linkGenerationService)
    {
        string baseUrl = linkGenerationService.Generate(
            action: nameof(PublicationsController.ListPublications),
            controller: nameof(PublicationsController),
            values: null
        )!;
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
