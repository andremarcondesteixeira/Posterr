using Posterr.Core.Application.UseCases.ListPublicationsWithPagination;
using Posterr.Presentation.Web.RestApi.Controllers.Publications;

namespace Posterr.Presentation.Web.RestApi.Tests.Controllers.Models;

public class ListPublicationsResponseItemDTOTests
{
    [Fact]
    public void GivenPublicationsPageEntryDTOInstance_ThenReturnNewListPublicationsResponseItemDTOInstance()
    {
        var postPublicationDate = DateTime.UtcNow.AddDays(-1);
        var repostPublicationDate = DateTime.UtcNow;
        var from = new PublicationsPageEntryDTO
        {
            IsRepost = true,
            Post = new(1, "original_author", postPublicationDate, "content"),
            Repost = new("reposter", repostPublicationDate)
        };
        var to = ListPublicationsResponseItemDTO.FromPublicationsPageEntryDTO(from);

        Assert.Equal(1, to.PostId);
        Assert.Equal("original_author", to.PostAuthorUsername);
        Assert.Equal(postPublicationDate, to.PostPublicationDate);
        Assert.Equal("content", to.PostContent);
        Assert.True(to.IsRepost);
        Assert.Equal("reposter", to.RepostAuthorUsername);
        Assert.Equal(repostPublicationDate, to.RepostPublicationDate);
    }
}
