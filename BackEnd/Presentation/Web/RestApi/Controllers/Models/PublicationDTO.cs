using Newtonsoft.Json;
using Posterr.Core.Application.UseCases.PaginatePublications;
using Posterr.Presentation.Web.RestApi.Controllers.HATEOAS;

namespace Posterr.Presentation.Web.RestApi.Controllers.Models;

public class PublicationDTO : APIResource
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

    public static PublicationDTO FromPaginatePublicationsResponseItemDTO(PaginatePublicationsResponseItemDTO dto)
    {
        return new PublicationDTO
        {
            IsRepost = dto.IsRepost,
            PostAuthorUsername = dto.Post.AuthorUsername,
            PostContent = dto.Post.Content,
            PostId = dto.Post.PostId,
            PostPublicationDate = dto.Post.PublicationDate,
            RepostAuthorUsername = dto.Repost?.AuthorUsername,
            RepostPublicationDate = dto.Repost?.PublicationDate
        };
    }
}
