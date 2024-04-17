using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.Core.Enums;

namespace TeamsMaker.Api.DataAccess.Config;


public class ProposalConfig : IEntityTypeConfiguration<Proposal>
{
    public void Configure(EntityTypeBuilder<Proposal> builder)
    {
        builder.ToTable(nameof(Proposal), DatabaseSchemas.Dbo);

        builder
            .OwnsOne(x => x.File);


        builder
            .HasMany(x => x.ApprovalRequests)
            .WithOne(y => y.Proposal)
            .HasForeignKey(y => y.ProposalId);

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .HasDefaultValue(ProposalStatusEnum.NoApproval);
    }
}
