using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Posterr.Core.Application;

public static class DependencyInjection
{
    // This will automatically register all use cases in the DI Container
    public static void ConfigureUseCases(this IServiceCollection services)
    {
        var useCases = typeof(DependencyInjection).Assembly
            .GetTypes()
            .Where(type =>
                !type.GetTypeInfo().IsInterface
                && !type.GetTypeInfo().IsAbstract
                && type != typeof(DependencyInjection)
                && type.Namespace != nameof(Exceptions)
             );

        foreach (var useCase in useCases)
        {
            services.AddTransient(useCase);
        }
    }
}
