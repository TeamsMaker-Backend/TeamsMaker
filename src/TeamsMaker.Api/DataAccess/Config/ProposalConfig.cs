using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.Core.Enums;

namespace TeamsMaker.Api.DataAccess.Config;


public class ProposalConfig : IEntityTypeConfiguration<Proposal>
{
    public void Configure(EntityTypeBuilder<Proposal> builder)
    {
        builder.ToTable(nameof(Proposal), DatabaseSchemas.Dbo);

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .HasDefaultValue(ProposalStatusEnum.NoApproval);
        
        builder
            .HasOne(x => x.Circle)
            .WithOne(y => y.Proposal)
            .HasForeignKey<Proposal>(x => x.CircleId)
            .IsRequired(true);

        builder
            .HasMany(x => x.ApprovalRequests)
            .WithOne(y => y.Proposal)
            .HasForeignKey(y => y.ProposalId);
    }
}
