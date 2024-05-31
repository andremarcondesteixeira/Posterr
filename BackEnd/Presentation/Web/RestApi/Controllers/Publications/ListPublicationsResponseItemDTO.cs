using Newtonsoft.Json;
using Posterr.Core.Application.UseCases.ListPublicationsWithPagination;
using Posterr.Presentation.Web.RestApi.Controllers.HATEOAS.HAL;

namespace Posterr.Presentation.Web.RestApi.Controllers.Publications;

public record ListPublicationsResponseItemDTO : APIResource
{
    public required bool IsRepost { get; init; }
    required public long PostId { get; init; }
    required public string PostAuthorUsername { get; init; }
    required public DateTime PostPublicationDate { get; init; }
    required public string PostContent { get; init; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? RepostAuthorUsername { get; init; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime? RepostPublicationDate { get; init; }

    public static ListPublicationsResponseItemDTO FromPublicationsPageEntryDTO(PublicationsPageEntryDTO dto)
    {
        return new ListPublicationsResponseItemDTO
        {
            IsRepost = dto.IsRepost,
            PostAuthorUsername = dto.Post.AuthorUsername,
            PostContent = dto.Post.Content,
            PostId = dto.Post.Id,
            PostPublicationDate = dto.Post.PublicationDate,
            RepostAuthorUsername = dto.Repost?.AuthorUsername,
            RepostPublicationDate = dto.Repost?.PublicationDate
        };
    }
}
