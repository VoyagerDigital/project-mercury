using Mercury.Modules.ActivityManagement.Domain.AcademicYears;
using Mercury.Shared.Infrastructure.Database.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mercury.Modules.ActivityManagement.Infrastructure.Database.Configurations.AcademicYears;

internal sealed class AcademicYearConfiguration : EntityConfiguration<AcademicYear>
{
    public override void Configure(EntityTypeBuilder<AcademicYear> builder)
    {
        base.Configure(builder);

        builder.HasMany(ay => ay.Activities)
            .WithOne();

        builder.Navigation(ay => ay.Activities)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        
        builder.Ignore(ay => ay.Name);
    }
}