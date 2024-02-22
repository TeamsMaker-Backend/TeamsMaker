using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;

namespace TeamsMaker.Api.DataAccess.Config;

public class ImportedStudentConfig : IEntityTypeConfiguration<ImportedStudent>
{
    public void Configure(EntityTypeBuilder<ImportedStudent> builder)
    {
        builder.ToTable(nameof(ImportedStudent), DatabaseSchemas.Lookups);

        builder.HasData(LoadData());
    }

    private ImportedStudent[] LoadData()
    {
        return [
            new ImportedStudent
            {
                Id = Guid.Parse("5cba5edb-d6f0-4dee-85df-7f23fcbf86d3"),
                SSN = "600-68-1014",
                CollegeId = "College-123",
                GPA = 3.5f,
                GraduationYear = DateOnly.FromDateTime(new DateTime(2026, 02, 17)),
                Department = "CS",
                OrganizationId = 1
            },
            new ImportedStudent
            {
                Id = Guid.Parse("86281c15-127d-4c91-9dff-dcc24164f79b"),
                SSN = "776-11-4808",
                CollegeId = "College-456",
                GPA = 3.3f,
                GraduationYear = DateOnly.FromDateTime(new DateTime(2024, 02, 17)),
                Department = "IS",
                OrganizationId = 1
            }
        ];
    }
}
