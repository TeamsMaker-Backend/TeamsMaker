using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;

namespace TeamsMaker.Api.DataAccess.Config
{
    public class ImportedStaffConfig : IEntityTypeConfiguration<ImportedStaff>
    {
        public void Configure(EntityTypeBuilder<ImportedStaff> builder)
        {
            builder.ToTable(nameof(ImportedStaff), DatabaseSchemas.Lookups);

            builder.HasData(LoadData());
        }

        private ImportedStaff[] LoadData()
        {
            return [
                new ImportedStaff
                {
                    Id = Guid.NewGuid(),
                    SSN = "553-35-8652",
                    OrganizationId = 1
                },
                new ImportedStaff
                {
                    Id = Guid.NewGuid(),
                    SSN = "622-45-0646",
                    OrganizationId = 1
                }
            ];
        }
    }
}
