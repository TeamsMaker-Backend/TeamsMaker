using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;

namespace TeamsMaker.Api.DataAccess.Config;

public class SessionConfig : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable(nameof(Session), DatabaseSchemas.Dbo);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .HasColumnType("nvarchar(500)")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.Notes)
            .HasColumnType("nvarchar(2000)")
            .HasMaxLength(2000);

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.Date)
            .IsRequired();

        builder
            .HasMany(s => s.TodoTasks)
            .WithOne(td => td.Session)
            .HasForeignKey(td => td.SessionId);

        builder
            .HasOne(s => s.Circle)
            .WithMany(c => c.Sessions)
            .HasForeignKey(s => s.CircleId)
            .IsRequired();
    }
}
