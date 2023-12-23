using DataAccess.Base.Interfaces;
using DataAccess.Interceptors;

namespace DataAccess.Context;

public class AppDBContext : BaseContext
{
    private readonly IUserInfo _userInfo;
    private readonly EntitySaveChangesInterceptor _saveChangesInterceptor;

    public AppDBContext(DbContextOptions<BaseContext> options, IUserInfo userInfo, EntitySaveChangesInterceptor saveChangesInterceptor) : base(options)
    {
        _userInfo = userInfo;
        _saveChangesInterceptor = saveChangesInterceptor;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.AddInterceptors(_saveChangesInterceptor);
    }
}