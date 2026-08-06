using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Users.Appliation;
using Users.Infrastructure;

namespace Host.Extensions;

public static class UsersServiceExtensions
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddUsersInfrastructure(configuration);

        return services;
    }
}
