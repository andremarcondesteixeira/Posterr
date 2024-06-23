using Posterr.Core.Shared.EntitiesInterfaces;
using Posterr.Presentation.Web.RestApi.Controllers.Publications;
using Posterr.Presentation.Web.RestApi.Controllers.SharedModels.HATEOAS.HAL;

namespace Posterr.Presentation.Web.RestApi.Controllers.SharedModels;

public sealed record PostAPIResourceDTO : APIResource<PostAPIResourceDTO.EmbeddedObjects>
{
    public bool IsRepost { get; } = false;
    public long Id { get; }
    public required string AuthorUsername { get; init; }
    public required DateTime PublicationDate { get; init; }
    public required string Content { get; init; }

    public PostAPIResourceDTO(long id, LinkGenerationService linkGenerationService)
    {
        string selfUrl = linkGenerationService.Generate(
            controller: nameof(PublicationsController),
            action: nameof(PublicationsController.GetPublicationById),
            values: new { publicationId = id }
        )!;
        APIResourceLinkDTO selfResourceLink = new(selfUrl);
        Id = id;
        Links.Add("self", [selfResourceLink]);
    }

    public static PostAPIResourceDTO FromIPost(IPost post, LinkGenerationService linkGenerationService)
    {
        UserAPIResourceDTO author = new(post.Author.Id, linkGenerationService)
        {
            Username = post.Author.Username
        };

        return new PostAPIResourceDTO(post.Id, linkGenerationService)
        {
            AuthorUsername = author.Username,
            PublicationDate = post.PublicationDate,
            Content = post.Content,
            Embedded = new EmbeddedObjects(author)
        };
    }

    public sealed record EmbeddedObjects(UserAPIResourceDTO Author);
}
