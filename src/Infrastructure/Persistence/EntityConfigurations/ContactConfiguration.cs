using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Contexts;

namespace Persistence.EntityConfigurations;

public class ContactConfiguration : BaseEntityConfiguration<Contact>
{
    public override void Configure(EntityTypeBuilder<Contact> builder)
    {
        base.Configure(builder);

        builder.Property(c => c.UserId).IsRequired();
        builder.Property(c => c.Email).HasMaxLength(50).IsRequired();
        builder.Property(c => c.FullName).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Message).HasMaxLength(255);

        builder.ToTable("contacts", InslariDbContext.DEFAULT_SCHEMA);
    }
}
