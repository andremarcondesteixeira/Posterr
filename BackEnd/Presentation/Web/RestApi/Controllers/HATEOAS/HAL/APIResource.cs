using System.Text.Json.Serialization;

namespace Posterr.Presentation.Web.RestApi.Controllers.HATEOAS.HAL;

// HAL Specification: https://datatracker.ietf.org/doc/html/draft-kelly-json-hal-11

public abstract record APIResource
{
    [JsonPropertyName("_links")]
    [JsonConverter(typeof(LinksConverter))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IDictionary<string, IList<APIResourceLinkDTO>> Links { get; } =
        new Dictionary<string, IList<APIResourceLinkDTO>>();
}

public abstract record APIResource<EMBEDDED> : APIResource
{
    [JsonPropertyName("_embedded")]
    public required EMBEDDED Embedded { get; init; }
}
