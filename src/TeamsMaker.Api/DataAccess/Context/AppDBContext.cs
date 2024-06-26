using System.Reflection;

using TeamsMaker.Api.DataAccess.Interceptors;

namespace TeamsMaker.Api.DataAccess.Context;

public class AppDBContext(DbContextOptions options,
    EntitySaveChangesInterceptor saveChangesInterceptor) : IdentityDbContext<User, Role, string>(options)
{

    #region DBSets
    public DbSet<Student> Students { get; set; }
    public DbSet<Staff> Staff { get; set; }
    public DbSet<ImportedStudent> ImportedStudents { get; set; }
    public DbSet<ImportedStaff> ImportedStaff { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Link> Links { get; set; }
    public DbSet<Experience> Experiences { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<Circle> Circles { get; set; }
    public DbSet<CircleMember> CircleMembers { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<JoinRequest> JoinRequests { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<TodoTask> TodoTasks { get; set; }
    public DbSet<Upvote> Upvotes { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<React> Reacts { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Proposal> Proposals { get; set; }
    public DbSet<ApprovalRequest> ApprovalRequests { get; set; }
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
        optionsBuilder.AddInterceptors(saveChangesInterceptor);
    }
}