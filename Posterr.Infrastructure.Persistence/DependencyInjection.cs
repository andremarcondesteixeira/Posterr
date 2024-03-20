using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Posterr.Core.Application.Boundaries.Persistence;
using Posterr.Infrastructure.Persistence.Repositories;

namespace Posterr.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static void ConfigurePersistence(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
        });
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IPublicationsRepository, PublicationsRepository>();
    }
}