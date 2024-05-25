using Posterr.Presentation.Web.RestApi.Controllers.HATEOAS;

namespace Posterr.Presentation.Web.RestApi.Controllers.Models;

public class PublicationsPaginationDTO : APIResource<PublicationDTO>
{
    public int Count { get; set; }

    public PublicationsPaginationDTO(IList<PublicationDTO> publications,
                                     int pageNumber,
                                     short pageSize,
                                     string baseUrl)
    {
        Count = publications.Count;
        Embedded.Add("publications", publications);

        Links.Add("self", [new($"{baseUrl}?pageNumber={pageNumber}")]);
        if (publications.Count >= pageSize)
        {
            Links.Add("next", [new($"{baseUrl}?pageNumber={pageNumber + 1}")]);
        }
    }
}
