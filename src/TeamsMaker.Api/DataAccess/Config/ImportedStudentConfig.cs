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
                Id = Guid.NewGuid(),
                SSN = "600-68-1014",
                CollegeId = "College-123",
                GPA = 3.5f,
                GraduationYear = DateOnly.FromDateTime(DateTime.Now),
                Department = "Computer Science",
                OrganizationId = 1
            },
            new ImportedStudent
            {
                Id = Guid.NewGuid(),
                SSN = "776-11-4808",
                CollegeId = "College-456",
                GPA = 3.3f,
                GraduationYear = DateOnly.FromDateTime(DateTime.Now.AddYears(2)),
                Department = "Information System",
                OrganizationId = 1
            }
        ];
    }
}
