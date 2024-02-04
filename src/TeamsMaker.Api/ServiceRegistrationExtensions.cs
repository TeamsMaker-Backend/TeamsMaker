using TeamsMaker.Api.DataAccess.Interceptors;
using TeamsMaker.Api.DataAccess.Context;
using DataAccess.Base.Interfaces;
using DataAccess.Base;
using TeamsMaker.Api.Services.Auth;
using TeamsMaker.Api.Services.Organizations;

namespace TeamsMaker.Api;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection RegisterDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        // add the DbContext
        services.AddDbContext<BaseContext>(
            options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    o => o
                        .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                        .LogTo(Console.WriteLine, LogLevel.Information));


        services.AddScoped<EntitySaveChangesInterceptor>();

        services.AddScoped<AppDBContext>();

        services.AddIdentity<User, Role>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
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
        services.AddScoped<IOrganizationService, OrganizationService>();


        return services;
    }
}
