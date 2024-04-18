using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;

namespace TeamsMaker.Api.DataAccess.Config;

public class AuthorConfig : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable(nameof(Author), DatabaseSchemas.Dbo);

        builder.HasKey(x => x.Id);

        builder
            .HasOne(x => x.Circle)
            .WithOne(y => y.Author)
            .HasForeignKey<Author>(x => x.CircleId)
            .IsRequired(false);

        builder
            .HasOne(x => x.User)
            .WithOne(y => y.Author)
            .HasForeignKey<Author>(x => x.UserId)
            .IsRequired(false);

        builder
            .HasMany(x => x.Posts)
            .WithOne(y => y.Author)
            .HasForeignKey(y => y.AuthorId)
            .IsRequired(true);
    }
}
