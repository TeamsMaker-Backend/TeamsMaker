using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;

namespace TeamsMaker.Api.DataAccess.Config
{
    public class ReactConfig : IEntityTypeConfiguration<React>
    {
        public void Configure(EntityTypeBuilder<React> builder)
        {
            builder.ToTable(nameof(React), DatabaseSchemas.Dbo);

            builder
                .HasOne(r => r.Post)
                .WithMany(r => r.Reacts)
                .HasForeignKey(r => r.PostId)
                .IsRequired(true);

            builder
                .HasOne(r => r.User)
                .WithMany(u => u.Reacts)
                .HasForeignKey(r => r.UserId)
                .IsRequired(false);
        }
    }
}
