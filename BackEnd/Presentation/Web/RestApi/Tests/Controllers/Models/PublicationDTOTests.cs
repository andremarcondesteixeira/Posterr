using Posterr.Core.Application.UseCases.PaginatePublications;
using Posterr.Presentation.Web.RestApi.Controllers.Models;

namespace Posterr.Presentation.Web.RestApi.Tests.Controllers.Models;

public class PublicationDTOTests
{
    [Fact]
    public void GivenPaginatePublicationsResponseItemDTOInstance_ThenReturnNewPublicationDTOInstance()
    {
        var postPublicationDate = DateTime.UtcNow.AddDays(-1);
        var repostPublicationDate = DateTime.UtcNow;
        var from = new PaginatePublicationsResponseItemDTO
        {
            IsRepost = true,
            Post = new(1, "original_author", postPublicationDate, "content"),
            Repost = new("reposter", repostPublicationDate)
        };
        var to = PublicationDTO.FromPaginatePublicationsResponseItemDTO(from);

        Assert.Equal(1, to.PostId);
        Assert.Equal("original_author", to.PostAuthorUsername);
        Assert.Equal(postPublicationDate, to.PostPublicationDate);
        Assert.Equal("content", to.PostContent);
        Assert.True(to.IsRepost);
        Assert.Equal("reposter", to.RepostAuthorUsername);
        Assert.Equal(repostPublicationDate, to.RepostPublicationDate);
    }
}
