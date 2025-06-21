using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portal.Models;

public abstract class BaseModelConfiguration<T> : IEntityTypeConfiguration<T>
    where T : BaseModel
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        // Primary Key, auto-increment
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        // MainId: Required, Default to empty string
        builder.Property(e => e.MainId)
            .IsRequired()
            .HasDefaultValue(string.Empty);

        // CreatedAt: Required, default value SQL, set at DB insert
        builder.Property(e => e.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // UpdatedAt: Required, default value SQL, updated by DB on update
        builder.Property(e => e.UpdatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAddOrUpdate();
    }
}

public abstract class BaseModelWithOnlyIdConfiguration<T> : IEntityTypeConfiguration<T>
    where T : BaseModelWithOnlyId
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        // Primary Key, auto-increment
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();
    }
}