using DataAccess.Base;
using DataAccess.Base.Interfaces;

using TeamsMaker.Api.Controllers.Circles;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.DataAccess.Interceptors;
using TeamsMaker.Api.Services.ApprovalRequests;
using TeamsMaker.Api.Services.ApprovalRequests.Interfaces;
using TeamsMaker.Api.Services.Auth;
using TeamsMaker.Api.Services.Circles;
using TeamsMaker.Api.Services.Circles.Interfaces;
using TeamsMaker.Api.Services.Departments;
using TeamsMaker.Api.Services.Departments.Interfaces;
using TeamsMaker.Api.Services.Files;
using TeamsMaker.Api.Services.Files.Interfaces;
using TeamsMaker.Api.Services.JoinRequests;
using TeamsMaker.Api.Services.JoinRequests.Interfaces;
using TeamsMaker.Api.Services.Organizations;
using TeamsMaker.Api.Services.Posts;
using TeamsMaker.Api.Services.Posts.Interfaces;
using TeamsMaker.Api.Services.Profiles;
using TeamsMaker.Api.Services.Profiles.Interfaces;
using TeamsMaker.Api.Services.Proposals;
using TeamsMaker.Api.Services.Proposals.Interfaces;
using TeamsMaker.Api.Services.Storage;
using TeamsMaker.Api.Services.Storage.Interfacecs;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection RegisterDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        // add the DbContext
        services.AddDbContext<AppDBContext>(
            options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), // ip address: 185.187.169.185
                    o => o
                        .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                        .LogTo(Console.WriteLine, LogLevel.Warning)
                        .EnableSensitiveDataLogging());

        services.AddScoped<EntitySaveChangesInterceptor>();

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
        services.AddScoped<IUserService, UserService>();

        services.AddSingleton<IStorageService, StorageService>();

        services.AddScoped<IOrganizationService, OrganizationService>();
        services.AddKeyedScoped<IFileService, OrganizationFileService>(BaseTypes.Organization);

        services.AddScoped<ProfileUtilities>();

        services.AddScoped<IProjectService, ProjectService>();

        services.AddScoped<IExperienceService, ExperienceService>();

        services.AddKeyedScoped<IProfileService, StudentProfileService>(UserEnum.Student);
        services.AddKeyedScoped<IFileService, StudentFileService>(BaseTypes.Student);

        services.AddKeyedScoped<IProfileService, StaffProfileService>(UserEnum.Staff);
        services.AddKeyedScoped<IFileService, StaffFileService>(BaseTypes.Staff);

        services.AddScoped<ICircleService, CircleService>();
        services.AddScoped<ICircleValidationService, CircleValidationService>();
        services.AddKeyedScoped<IFileService, CircleFileService>(BaseTypes.Circle);
        services.AddKeyedScoped<IPermissionService, CircleService>(BaseTypes.Circle);

        services.AddScoped<ICircleMemberService, CircleMemberService>();
        services.AddKeyedScoped<IPermissionService, CircleMemberService>(BaseTypes.CircleMember);

        services.AddScoped<IJoinRequestService, JoinRequestService>();

        services.AddScoped<ISessionService, SessionService>();

        services.AddScoped<ITodoTaskService, TodoTaskSerevice>();

        services.AddScoped<IProposalService, ProposalService>();

        services.AddScoped<IApprovalRequestService, ApprovalRequestService>();

        services.AddScoped<IPostService, PostService>();

        services.AddScoped<IDepartmentService, DepartmentService>();

        return services;
    }
}
