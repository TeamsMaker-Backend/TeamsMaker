using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;

namespace TeamsMaker.Api.DataAccess.Config;

public class PostConfig : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable(nameof(Post), DatabaseSchemas.Dbo);

        builder.HasKey(x => x.Id);

        builder
            .HasOne(x => x.Author)
            .WithMany(y => y.Posts)
            .HasForeignKey(x => x.AuthorId)
            .IsRequired(true);

        builder
            .HasMany(p => p.Comments)
            .WithOne(c => c.ParentPost)
            .HasForeignKey(c => c.ParentPostId)
            .IsRequired(false);

        builder
            .HasOne(x => x.Author)
            .WithMany(y => y.Posts)
            .HasForeignKey(x => x.AuthorId)
            .IsRequired(true);
    }
}
