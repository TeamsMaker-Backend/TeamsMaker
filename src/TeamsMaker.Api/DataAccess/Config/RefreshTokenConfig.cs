using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.DataAccess.Models;

namespace TeamsMaker.Api.DataAccess.Config;

public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasOne(x => x.User)
            .WithMany(y => y.RefreshTokens)
            .HasForeignKey(x => x.UserId);
    }
}
