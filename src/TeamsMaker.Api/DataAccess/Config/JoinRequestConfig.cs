using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;

namespace TeamsMaker.Api.DataAccess.Config;

public class JoinRequestConfig : IEntityTypeConfiguration<JoinRequest>
{
    public void Configure(EntityTypeBuilder<JoinRequest> builder)
    {
        builder.ToTable(nameof(JoinRequest), DatabaseSchemas.Dbo);

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.IsAccepted)
            .HasDefaultValue(false);

        builder
            .Property(x => x.Sender)
            .HasMaxLength(6);

        builder
           .HasOne(x => x.Circle)
           .WithMany(y => y.Invitions)
           .HasForeignKey(x => x.CircleId)
           .IsRequired();

        builder
           .HasOne(x => x.Student)
           .WithMany(y => y.JoinRequests)
           .HasForeignKey(x => x.StudentId)
           .IsRequired();
    }
}
