namespace DataAccess.Context;

public class BaseContext : IdentityDbContext<User, IdentityRole, string>
{
    public BaseContext(DbContextOptions<BaseContext> option) : base(option)
    { }

    #region DBSets

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Arabic_CI_AS");

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

    public override int SaveChanges() => base.SaveChanges();

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
         => base.SaveChangesAsync(cancellationToken);
}
