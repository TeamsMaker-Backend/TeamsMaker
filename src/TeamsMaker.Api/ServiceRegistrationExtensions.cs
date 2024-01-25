using DataAccess.Base;
using DataAccess.Base.Interfaces;
using DataAccess.Context;
using DataAccess.Interceptors;

namespace TeamsMaker.Api;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection RegisterDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<EntitySaveChangesInterceptor>();

        // add the DbContext
        services.AddDbContext<AppDBContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<AppDBContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    public static IServiceCollection RegisterBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IUserInfo, UserInfo>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
