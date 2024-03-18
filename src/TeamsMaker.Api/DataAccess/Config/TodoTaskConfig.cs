using Humanizer;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;

namespace TeamsMaker.Api.DataAccess.Config;

public class TodoTaskConfig : IEntityTypeConfiguration<TodoTask>
{
    public void Configure(EntityTypeBuilder<TodoTask> builder)
    {
        builder.ToTable(nameof(TodoTask), DatabaseSchemas.Dbo);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .HasColumnType("nvarchar(500)")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.Notes)
            .HasColumnType("nvarchar(2000)")
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.DeadLine)
            .IsRequired();

        builder
            .HasOne(td => td.Session)
            .WithMany(s => s.TodoTasks)
            .HasForeignKey(td => td.SessionId);

        builder
            .HasOne(td => td.Circle)
            .WithMany(s => s.TodoTasks)
            .HasForeignKey(td => td.CircleId)
            .IsRequired();
    }
}
