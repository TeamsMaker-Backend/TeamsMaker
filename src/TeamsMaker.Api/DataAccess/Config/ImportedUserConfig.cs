using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;

namespace TeamsMaker.Api.DataAccess.Config;

public class ImportedUserConfig : IEntityTypeConfiguration<ImportedUser>
{
    public void Configure(EntityTypeBuilder<ImportedUser> builder)
    {
        builder.ToTable(nameof(ImportedUser), DatabaseSchemas.Lookups);
    }
}
