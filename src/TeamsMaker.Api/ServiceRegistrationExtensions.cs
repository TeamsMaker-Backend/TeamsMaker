using DataAccess.Base;
using DataAccess.Base.Interfaces;
using DataAccess.Context;
using DataAccess.Interceptors;

namespace TeamsMaker.Api;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection RegisterDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        // add the DbContext
        services.AddDbContext<BaseContext>(options =>
            options.UseLazyLoadingProxies().UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
        );
        services.AddScoped(typeof(AppDBContext));

        #region hangfire
        // services.AddHangfire(config => config
        //     .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        //     .UseSimpleAssemblyNameTypeSerializer()
        //     .UseRecommendedSerializerSettings()
        //     .UseSqlServerStorage(configuration.GetConnectionString("SqlConnection"), new SqlServerStorageOptions
        //     {
        //         CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        //         SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        //         QueuePollInterval = TimeSpan.Zero,
        //         UseRecommendedIsolationLevel = true,
        //         DisableGlobalLocks = true
        //     }));
        #endregion

        return services;
    }

    public static IServiceCollection RegisterBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IUserInfo, UserInfo>();

        return services;
    }
}
