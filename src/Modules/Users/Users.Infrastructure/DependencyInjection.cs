using BuildingBlocks.Interfaces;
using BuildingBlocks.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Users.Appliation.Interfaces.Repositories;
using Users.Appliation.Interfaces.Services;
using Users.Infrastructure.Data;
using Users.Infrastructure.Identity;
using Users.Infrastructure.Repositories;
using Users.Infrastructure.Services;

namespace Users.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        #region Database
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        #endregion

        #region Identity
        services.AddIdentity<UserIdentity, RoleIdentity>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        #endregion

        #region HTTP Context
        services.AddHttpContextAccessor();
        #endregion

        #region Services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserContext, UserContext>();
        #endregion

        #region Repositories
        services.AddScoped<ITokenRepository, TokenRepository>();
        #endregion

        return services;
    }
}
