using System.Reflection;

namespace TeamsMaker.Api.DataAccess.Context;

public class BaseContext(DbContextOptions<BaseContext> options) : IdentityDbContext<User, Role, string>(options)
{
    #region DBSets
    public DbSet<Student> Students { get; set; }
    public DbSet<Staff> Staff { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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

    // public override int SaveChanges() => base.SaveChanges();
    // public override Task<int> SaveChangesAsync(CancellationToken ct = default) => base.SaveChangesAsync(ct);
}
