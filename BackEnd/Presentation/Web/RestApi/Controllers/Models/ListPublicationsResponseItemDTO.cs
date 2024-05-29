using Newtonsoft.Json;
using Posterr.Core.Application.UseCases.ListPublicationsWithPagination;
using Posterr.Presentation.Web.RestApi.Controllers.HATEOAS.HAL;

namespace Posterr.Presentation.Web.RestApi.Controllers.Models;

public class ListPublicationsResponseItemDTO : APIResource
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

    public ListPublicationsResponseItemDTO(string baseUrl)
    {
        // Since the test does not ask for an endpoint for individual posts or reposts,
        // then I'm not gonna put a "self" link here
        //Links.Add("self", [new(baseUrl)]);
    }

    public static ListPublicationsResponseItemDTO FromPublicationsPageEntryDTO(PublicationsPageEntryDTO dto, string baseUrl)
    {
        return new ListPublicationsResponseItemDTO(baseUrl)
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
