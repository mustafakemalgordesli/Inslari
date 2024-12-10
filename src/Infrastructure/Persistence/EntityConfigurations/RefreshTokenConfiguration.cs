using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Contexts;

namespace Persistence.EntityConfigurations;

public class RefreshTokenConfiguration : BaseEntityConfiguration<RefreshToken>
{
    public override void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        base.Configure(builder);

        builder.Property(r => r.Token).HasMaxLength(200);
        builder.HasIndex(r => r.Token).IsUnique();

        builder.ToTable("refreshtokens", InslariDbContext.DEFAULT_SCHEMA);
    }
}
