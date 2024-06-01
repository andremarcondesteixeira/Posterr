using Microsoft.Extensions.DependencyInjection;

namespace Posterr.Core.Application.UseCases;

public static class DependencyInjection
{
    // This will automatically register all use cases in the DI Container
    public static void ConfigureUseCases(this IServiceCollection services)
    {
        typeof(DependencyInjection)
            .Assembly
            .GetTypes()
            .Where(type =>
                !type.IsInterface
                && (type?.FullName?.Contains(typeof(DependencyInjection).Namespace!) ?? true)
                && (type?.IsAssignableTo(typeof(IUseCaseRegisteredInDependencyInjectionContainer)) ?? false)
            )
            .ToList()
            .ForEach(useCase => services.AddTransient(useCase));
    }
}
