using Posterr.Core.Application.UseCases.ListPublicationsWithPagination;
using Posterr.Presentation.Web.RestApi.Controllers.HATEOAS.HAL;
using Posterr.Presentation.Web.RestApi.Controllers.Models;

namespace Posterr.Presentation.Web.RestApi.Controllers.Publications;

public sealed record PublicationsListDTO : APIResource<PublicationsListDTO.PublicationsList>
{
    public int Count { get; set; }

    public PublicationsListDTO(
        IList<PublicationsListItemDTO> publications,
        ListPublicationsUseCaseInputDTO paginationParameters,
        string baseUrl)
    {
        Count = publications.Count;
        Links.Add("self", [new($"{baseUrl}?pageNumber={paginationParameters.PageNumber}")]);
        
        if (publications.Count >= paginationParameters.PageSize)
        {
            Links.Add("next", [new($"{baseUrl}?pageNumber={paginationParameters.PageNumber + 1}")]);
        }
    }

    public sealed record PublicationsList
    {
        public required List<PublicationsListItemDTO> Publications { get; init; }
    }

    public sealed record PublicationsListItemDTO : APIResource<PublicationsListItemDTO.EmbeddedObjects>
    {
        public required long Id { get; init; }
        public required DateTime PublicationDate { get; init; }
        public required string Content { get; init; }

        public sealed record EmbeddedObjects(UserAPIResourceDTO Author, PostAPIResourceDTO? OriginalPost);
    }
}
