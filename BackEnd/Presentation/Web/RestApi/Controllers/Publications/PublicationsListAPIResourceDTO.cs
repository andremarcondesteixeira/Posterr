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
        Links.Add("self", [new($"{baseUrl}?lastSeenPublicationId={paginationParameters.LastSeenPublicationId}&isFirstPage={paginationParameters.IsFirstPage}")]);

        if (publications.Count >= paginationParameters.PageSize)
        {
            var oldestPublication = publications.Last();
            if (oldestPublication is not null)
            {
                Links.Add("next", [new($"{baseUrl}?lastSeenPublicationId={oldestPublication.Id}&isFirstPage=false")]);
            }
        }
    }

    public sealed record EmbeddedObjects
    {
        public required List<PublicationAPIResourceDTO> Publications { get; init; }
    }
}
