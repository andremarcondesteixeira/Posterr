namespace Posterr.Presentation.Web.RestApi.Controllers.HATEOAS;

// HAL Specification: https://datatracker.ietf.org/doc/html/draft-kelly-json-hal-11

public abstract class APIResource
{
    public Dictionary<string, List<APIResourceLinkDTO>> Links { get; } = [];
}

public abstract class APIResource<EMBEDDED> : APIResource where EMBEDDED : APIResource {
    public Dictionary<string, IList<EMBEDDED>> Embedded { get; } = [];
}
