using Posterr.Core.Application.UseCases.ListPublicationsWithPagination;
using Posterr.Presentation.Web.RestApi.Controllers.HATEOAS.HAL;

namespace Posterr.Presentation.Web.RestApi.Controllers.Models;

public class ListPublicationsResponseDTO : APIResource<ListPublicationsResponseItemDTO>
{
    public int Count { get; set; }

    public ListPublicationsResponseDTO(IList<ListPublicationsResponseItemDTO> publications,
                                     PaginationParameters paginationParameters,
                                     string baseUrl)
    {
        Count = publications.Count;

        Embedded.Add("publications", publications);

        Links.Add("self", [new($"{baseUrl}?pageNumber={paginationParameters.PageNumber}")]);
        if (publications.Count >= paginationParameters.PageSize)
        {
            Links.Add("next", [new($"{baseUrl}?pageNumber={paginationParameters.PageNumber + 1}")]);
        }
    }
}
