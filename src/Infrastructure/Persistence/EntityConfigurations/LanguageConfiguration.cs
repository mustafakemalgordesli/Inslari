using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Contexts;

namespace Persistence.EntityConfigurations;

public class LanguageConfiguration : BaseEntityConfiguration<Language>
{
    public override void Configure(EntityTypeBuilder<Language> builder)
    {
        base.Configure(builder);

        builder.Property(l => l.Code).HasMaxLength(10).IsRequired();
        builder.HasIndex(l => l.Code).IsUnique();

        builder.Property(l => l.Name).HasMaxLength(100).IsRequired();
        builder.Property(l => l.IsDefault).HasDefaultValue(false);
        builder.Property(l => l.IsActive).HasDefaultValue(true);

        builder.ToTable("languages", InslariDbContext.DEFAULT_SCHEMA);
    }
}
