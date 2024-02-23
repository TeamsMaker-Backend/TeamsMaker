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
            return
            [
                new ImportedStaff
                {
                    Id = Guid.Parse("3e9f4430-2927-41eb-a8a5-099248d1e6ba"),
                    SSN = "553-35-8652",
                    OrganizationId = 1
                },
                new ImportedStaff
                {
                    Id = Guid.Parse("9266966b-fa8e-461a-bd61-0a1a15d5c234"),
                    SSN = "622-45-0646",
                    OrganizationId = 1
                }
            ];
        }
    }
}
