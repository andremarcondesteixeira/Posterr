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
        var from = new ListPublicationsUseCaseOutputListItemDTO
        {
            IsRepost = true,
            Post = new(1, "original_author", postPublicationDate, "content"),
            Repost = new("reposter", repostPublicationDate)
        };
        var to = ListPublicationsRequestResponseItemDTO.FromPublicationsPageEntryDTO(from);

        Assert.Equal(1, to.Id);
        Assert.Equal("original_author", to.AuthorUsername);
        Assert.Equal(postPublicationDate, to.PublicationDate);
        Assert.Equal("content", to.Content);
        Assert.True(to.IsRepost);
        Assert.Equal("reposter", to.RepostAuthorUsername);
        Assert.Equal(repostPublicationDate, to.RepostPublicationDate);
    }
}
