using System.Text.Json.Serialization;

namespace Posterr.Presentation.Web.RestApi.Controllers.HATEOAS.HAL;

// HAL Specification: https://datatracker.ietf.org/doc/html/draft-kelly-json-hal-11

public abstract class APIResource
{
    [JsonPropertyName("_links")]
    [JsonConverter(typeof(LinksConverter))]
    public IDictionary<string, IList<APIResourceLinkDTO>> Links { get; } =
        new Dictionary<string, IList<APIResourceLinkDTO>>();
}

public abstract class APIResource<EMBEDDED> : APIResource where EMBEDDED : APIResource
{
    [JsonPropertyName("_emdedded")]
    public Dictionary<string, IList<EMBEDDED>> Embedded { get; } = [];
}
