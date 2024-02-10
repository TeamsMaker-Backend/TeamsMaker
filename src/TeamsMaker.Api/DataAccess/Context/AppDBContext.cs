using System.Reflection;

using DataAccess.Base.Interfaces;

using TeamsMaker.Api.DataAccess.Interceptors;

namespace TeamsMaker.Api.DataAccess.Context;

public class AppDBContext : IdentityDbContext<User, Role, string>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly EntitySaveChangesInterceptor _saveChangesInterceptor;


    public AppDBContext(DbContextOptions options,
        IServiceProvider serviceProvider,
        EntitySaveChangesInterceptor saveChangesInterceptor) : base(options)
    {
        _saveChangesInterceptor = saveChangesInterceptor;
        _serviceProvider = serviceProvider;
    }

    private IUserInfo _userInfo => _serviceProvider.GetRequiredService<IUserInfo>();


    #region DBSets
    public DbSet<Student> Students { get; set; }
    public DbSet<Staff> Staff { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Department> Departments { get; set; }
    #endregion


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<Organization>().HasQueryFilter(b => b.Id == _userInfo.OrganizationId);
        // modelBuilder.Entity<User>().HasQueryFilter(b => b.OrganizationId == _userInfo.OrganizationId);
        // modelBuilder.Entity<Role>().HasQueryFilter(b => b.OrganizationId == _userInfo.OrganizationId);

        // Loads ****Config.cs files
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        /*
            For each of these foreign keys, it changes the delete behavior to DeleteBehavior.Restrict. 
            This means that if there are entities linked by a foreign key and an attempt is made to delete the 
            entity on the principal side, the deletion will be restricted unless all dependent entities are first deleted 
            or their foreign key values are set to null.
        */
        var cascadeFKs = modelBuilder
            .Model
            .GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (var fk in cascadeFKs)
            fk.DeleteBehavior = DeleteBehavior.Restrict;

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.AddInterceptors(_saveChangesInterceptor);
    }
}