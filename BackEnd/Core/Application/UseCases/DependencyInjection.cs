﻿using Microsoft.Extensions.DependencyInjection;
using Posterr.Core.Domain.Entities;

namespace Posterr.Core.Application.UseCases;

public static class DependencyInjection
{
    // This will automatically register all use cases in the DI Container
    public static void ConfigureUseCases(this IServiceCollection services)
    {
        services.AddScoped<IDomainPersistencePort, DomainPersistenceAdapter>();

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
