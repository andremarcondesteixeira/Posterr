using Posterr.Presentation.Web.RestApi.Controllers.HATEOAS.HAL;

namespace Posterr.Presentation.Web.RestApi.Controllers.Publications;

public record CreateNewPostRequestResponseDTO : APIResource
{
    public required long Id { get; init; }
    public required string AuthorUsername { get; init; }
    public required DateTime PublicationDate { get; init; }
    public required string Content { get; init; }

    public CreateNewPostRequestResponseDTO(string baseUrl)
    {
        Links.Add("self", [new(baseUrl)]);
    }
}
