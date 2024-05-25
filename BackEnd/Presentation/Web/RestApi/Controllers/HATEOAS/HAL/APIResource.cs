using Posterr.Presentation.Web.RestApi.Controllers.Models;

namespace Posterr.Presentation.Web.RestApi.Controllers.HATEOAS;

// HAL Specification: https://datatracker.ietf.org/doc/html/draft-kelly-json-hal-11

public abstract class APIResource
{
    public Dictionary<string, List<APIResourceLinkDTO>> Links { get; } = [];
    
    // There is no error property in the HAL specification.
    // I've put it here to facilitate returning error states in the Controllers.
    // If all resources can have errors, then I don't need to change the type of the return statement when errors happen.
    public IList<Error> Errors { get; } = [];
}

public abstract class APIResource<EMBEDDED> : APIResource where EMBEDDED : APIResource {
    public Dictionary<string, IList<EMBEDDED>> Embedded { get; } = [];
}
