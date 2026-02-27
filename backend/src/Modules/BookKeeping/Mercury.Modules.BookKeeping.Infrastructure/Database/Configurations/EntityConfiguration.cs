using Mercury.Shared.Kernel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mercury.Modules.BookKeeping.Infrastructure.Database.Configurations;

internal abstract class EntityConfiguration<T> : IEntityTypeConfiguration<T> 
    where T : Entity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.ToTable(typeof(T).Name);
        
        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(x => x.UpdatedAt)
            .IsConcurrencyToken();
        
        builder.HasKey(x => x.Id);
        
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}