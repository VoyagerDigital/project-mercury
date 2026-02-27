using Mercury.Modules.ActivityManagement.Domain.AcademicYears;
using Mercury.Modules.ActivityManagement.Domain.Activities;
using Microsoft.EntityFrameworkCore;

namespace Mercury.Modules.ActivityManagement.Infrastructure.Database;

public class ActivityManagementDbContext(DbContextOptions<ActivityManagementDbContext> options) : DbContext
{
    public DbSet<AcademicYear> AcademicYears => Set<AcademicYear>();
    public DbSet<Activity> Activities => Set<Activity>();
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>()
            .HavePrecision(18, 2);

        configurationBuilder.Properties<string>()
            .HaveMaxLength(4_000);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.HasDefaultSchema("activity_management");
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ActivityManagementDbContext).Assembly);
    }
}