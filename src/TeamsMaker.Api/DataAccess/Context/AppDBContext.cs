using TeamsMaker.Api.DataAccess.Interceptors;
using DataAccess.Base.Interfaces;

namespace TeamsMaker.Api.DataAccess.Context;

public class AppDBContext : BaseContext
{
    private readonly IUserInfo _userInfo;
    private readonly EntitySaveChangesInterceptor _saveChangesInterceptor;


    public AppDBContext(DbContextOptions<BaseContext> options,
        IUserInfo userInfo,
        EntitySaveChangesInterceptor saveChangesInterceptor) : base(options)
    {
        _userInfo = userInfo;
        _saveChangesInterceptor = saveChangesInterceptor;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Organization>().HasQueryFilter(b => b.Id == _userInfo.OrganizationId);
        modelBuilder.Entity<User>().HasQueryFilter(b => b.OrganizationId == _userInfo.OrganizationId);
        modelBuilder.Entity<Role>().HasQueryFilter(b => b.OrganizationId == _userInfo.OrganizationId);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.AddInterceptors(_saveChangesInterceptor);
    }
}