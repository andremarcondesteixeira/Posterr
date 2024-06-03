using Posterr.Core.Application.UseCases.ListPublicationsWithPagination;
using Posterr.Presentation.Web.RestApi.Controllers.HATEOAS.HAL;
using Posterr.Presentation.Web.RestApi.Controllers.Models;
using System.Text.Json.Serialization;

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
        public required bool IsRepost { get; init; }
        public long Id { get; }
        public required string AuthorUsername { get; init; }
        public required DateTime PublicationDate { get; init; }
        public required string Content { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? OriginalPostAuthorUsername { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? OriginalPostPublicationDate { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? OriginalPostContent { get; init; }

        public PublicationsListItemDTO(long id, string publicationsListEndpointUrl)
        {
            Id = id;
            Links.Add("self", [new($"{publicationsListEndpointUrl}/{Id}")]);
        }

        public sealed record EmbeddedObjects(UserAPIResourceDTO Author, PostAPIResourceDTO? OriginalPost);
    }
}
