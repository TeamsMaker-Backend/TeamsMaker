﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;

namespace TeamsMaker.Api.DataAccess.Config;

public class LinkConfig : IEntityTypeConfiguration<Link>
{
    public void Configure(EntityTypeBuilder<Link> builder)
    {
        builder.ToTable(nameof(Link), DatabaseSchemas.Dbo);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Url)
            .HasColumnType("nvarchar(2083)")
            .HasMaxLength(2083)
            .IsRequired();

        builder.Property(x => x.Type)
            .IsRequired();

        builder
            .HasOne(x => x.User)
            .WithMany(y => y.Links)
            .HasForeignKey(x => x.UserId)
            .IsRequired(false);

        builder
            .HasOne(x => x.Circle)
            .WithMany(y => y.Links)
            .HasForeignKey(x => x.CircleId)
            .IsRequired(false);
    }
}
