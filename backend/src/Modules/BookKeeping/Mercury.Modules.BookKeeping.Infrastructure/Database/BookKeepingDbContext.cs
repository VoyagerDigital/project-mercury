using Mercury.Modules.BookKeeping.Domain.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Mercury.Modules.BookKeeping.Infrastructure.Database;

public sealed class BookKeepingDbContext(DbContextOptions<BookKeepingDbContext> options) : DbContext(options)
{
    public DbSet<Transaction> Transactions => Set<Transaction>();

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
        
        modelBuilder.HasDefaultSchema("book_keeping");
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookKeepingDbContext).Assembly);
    }
}