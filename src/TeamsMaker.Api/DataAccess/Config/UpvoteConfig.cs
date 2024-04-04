using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;

namespace TeamsMaker.Api.DataAccess.Config;

public class UpvoteConfig : IEntityTypeConfiguration<Upvote>
{
    public void Configure(EntityTypeBuilder<Upvote> builder)
    {
        builder.ToTable(nameof(Upvote), DatabaseSchemas.Dbo);

        builder
            .HasOne(x => x.User)
            .WithMany(y => y.Upvotes)
            .HasForeignKey(x => x.UserId);

        builder
            .HasOne(x => x.Circle)
            .WithMany(y => y.Upvotes)
            .HasForeignKey(x => x.CircleId);
    }
}
