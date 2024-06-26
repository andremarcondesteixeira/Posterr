using Posterr.Core.Application.UseCases.SearchPublications;
using Posterr.Presentation.Web.RestApi.Controllers.SharedModels.HATEOAS.HAL;

namespace Posterr.Presentation.Web.RestApi.Controllers.Publications;

public record SearchResultsAPIResourceDTO : APIResource<SearchResultsAPIResourceDTO.EmbeddedObjects>
{
    public int Count { get => Embedded.Publications.Count; }

    public SearchResultsAPIResourceDTO(SearchPublicationsUseCaseInputDTO searchPublicationsUseCaseInput, LinkGenerationService linkGenerationService)
    {
        string baseUrl = linkGenerationService.Generate(
            action: nameof(PublicationsController.SearchPublications),
            controller: nameof(PublicationsController),
            values: null
        )!;
        Links.Add("self", [new($"{baseUrl}?searchTerm={searchPublicationsUseCaseInput.SearchTerm}")]);
    }

    public sealed record EmbeddedObjects
    {
        public required List<PublicationAPIResourceDTO> Publications { get; init; }
    }
}
