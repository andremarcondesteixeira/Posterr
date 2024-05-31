using Posterr.Presentation.Web.RestApi.Controllers.HATEOAS.HAL;

namespace Posterr.Presentation.Web.RestApi.Controllers.Publications;

public record CreateNewRepostRequestResponseDTO : APIResource<CreateNewRepostRequestResponseDTO.OriginalPostData>
{
    public required string AuthorUsername { get; init; }
    public required DateTime PublicationDate { get; init; }

    public CreateNewRepostRequestResponseDTO(OriginalPostData originalPostData, string baseUrl)
    {
        Embedded.Add("originalPost", [originalPostData]);
        Links.Add("self", [new(baseUrl)]);
    }

    public sealed record OriginalPostData : APIResource
    {
        public required long Id { get; init; }
        public required string AuthorUsername { get; init; }
        public required DateTime PublicationDate { get; init; }
        public required string Content { get; init; }
    }
}
