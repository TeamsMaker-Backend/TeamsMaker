using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;

namespace TeamsMaker.Api.DataAccess.Config;

public class ApprovalRequestConfig : IEntityTypeConfiguration<ApprovalRequest>
{
    public void Configure(EntityTypeBuilder<ApprovalRequest> builder)
    {
        builder.ToTable(nameof(ApprovalRequest), DatabaseSchemas.Dbo);

        builder.Property(x => x.Destination)
            .HasConversion<int>();

        builder.Property(x => x.Position)
            .HasConversion<int>();

        builder
            .HasOne(x => x.Proposal)
            .WithMany(y => y.ApprovalRequests)
            .HasForeignKey(x => x.ProposalId)
            .IsRequired(true);

        builder
            .HasOne(x => x.Staff)
            .WithMany(y => y.ApprovalRequests)
            .HasForeignKey(x => x.StaffId)
            .IsRequired(true);

        builder
            .HasOne(x => x.Supervisor)
            .WithMany(y => y.AcceptedApprovalRequests)
            .HasForeignKey(x => x.SupervisorId)
            .IsRequired(false);
    }
}
