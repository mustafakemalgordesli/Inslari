﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Contexts;

namespace Persistence.EntityConfigurations;

public class UserConfiguration : BaseEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Username).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Password).IsRequired();
        builder.Property(x => x.Email).IsRequired().HasMaxLength(255);

        builder.HasQueryFilter(x => x.IsDeleted == false);

        builder.ToTable("users", InslariDbContext.DEFAULT_SCHEMA);
    }
}
