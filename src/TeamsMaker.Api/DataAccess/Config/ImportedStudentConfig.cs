using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;

namespace TeamsMaker.Api.DataAccess.Config;

public class ImportedStudentConfig : IEntityTypeConfiguration<ImportedStudent>
{
    public void Configure(EntityTypeBuilder<ImportedStudent> builder)
    {
        builder.ToTable(nameof(ImportedStudent), DatabaseSchemas.Lookups);
    }
}
