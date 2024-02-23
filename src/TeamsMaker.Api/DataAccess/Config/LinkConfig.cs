using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;

namespace TeamsMaker.Api.DataAccess.Config
{
    public class LinkConfig : IEntityTypeConfiguration<Link>
    {
        public void Configure(EntityTypeBuilder<Link> builder)
        {
            builder.ToTable(nameof(Link), DatabaseSchemas.Dbo);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Url)
                .HasColumnType("nvarchar(2083)")
                .HasMaxLength(2083)
                .IsRequired();

            // Relation with Student One to Many
            builder.HasOne(x => x.Student)
                .WithMany(x => x.Links)
                .HasForeignKey(x => x.StudentId)
                .IsRequired();
        }
    }
}
