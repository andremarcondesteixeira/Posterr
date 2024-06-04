using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Posterr.Presentation.Web.RestApi.Controllers;

public class LinkGenerationService(IHttpContextAccessor ContextAcessor, LinkGenerator Generator)
{
    public string Generate(string controller, string action, object? values) => Generator.GetUriByAction(
        httpContext: ContextAcessor.HttpContext!,
        action: action,
        controller: controller.Replace("Controller", ""),
        values: values
    )!;
}
