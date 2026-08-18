using BuildingBlocks.Interfaces;
using BuildingBlocks.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks;

public static class DependencyInjection
{
    public static IServiceCollection AddBuildingBlocks(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        
        services.AddScoped<IUserContext, UserContext>();

        return services;
    }
}