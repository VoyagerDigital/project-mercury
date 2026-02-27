using Mercury.Modules.BookKeeping.Domain.Transactions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mercury.Modules.BookKeeping.Infrastructure.Database.Configurations.Transactions;

internal class TransactionConfiguration : EntityConfiguration<Transaction>
{
    public override void Configure(EntityTypeBuilder<Transaction> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.Type)
            .HasConversion<string>();
    }
}