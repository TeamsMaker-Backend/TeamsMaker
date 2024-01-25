using DataAccess.Base.Interfaces;
using DataAccess.Interceptors;
using TeamsMaker.Api.DataAccess.Models;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Context;

public class AppDBContext : IdentityDbContext<User, Role, Ulid>
{
    // private readonly IUserInfo _userInfo;
    // private readonly EntitySaveChangesInterceptor _saveChangesInterceptor;
    // private readonly IConfiguration _configuration;


    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    {
    }

    // public AppDBContext(DbContextOptions<AppDBContext> options,
    //     IUserInfo userInfo,
    //     EntitySaveChangesInterceptor saveChangesInterceptor,
    //     IConfiguration configuration) : base(options)
    // {
    //     _userInfo = userInfo;
    //     _saveChangesInterceptor = saveChangesInterceptor;
    //     _configuration = configuration;

    // }

    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Organization> Organizations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Loads ****Config.cs files
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDBContext).Assembly);

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
        // optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        // optionsBuilder.AddInterceptors(_saveChangesInterceptor);
    }
}