using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Posterr.Presentation.Web.RestApi.Controllers.HATEOAS.HAL;
using System.Reflection;

namespace Posterr.Presentation.Web.RestApi.Controllers;

public static class DependencyInjection
{
    // The controllers are in a separate project in order to make it impossible
    // to directly use the database repositories through the controllers.
    // All interactions of the application should go through a use case instead.
    // If I didn't separate the projects, the controllers could just ask for the
    // DbContext that was inserted in the dependency injection container.
    public static void ConfigureControllers(this IServiceCollection services)
    {
        Assembly assembly = typeof(DependencyInjection).Assembly;
        AssemblyPart part = new(assembly);
        services.AddControllers()
                .ConfigureApplicationPartManager(apm => apm.ApplicationParts.Add(part))
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new LinksConverter());
                });
    }
}
