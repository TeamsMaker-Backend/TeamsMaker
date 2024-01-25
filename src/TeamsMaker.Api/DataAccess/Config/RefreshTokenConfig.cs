using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.DataAccess.Models;

namespace TeamsMaker.Api.DataAccess.Config;

public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .HasConversion(
                ulid => ulid.ToString(),  // Convert ULID to string for the database
                str => Ulid.Parse(str)    // Convert string from the database to ULID
            )
            .IsRequired();

        builder
            .HasOne(x => x.User)
            .WithMany(y => y.RefreshTokens)
            .HasForeignKey(x => x.UserId);
    }
}
