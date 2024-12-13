using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Domain.Entities;
using Domain.Common;
using MediatR;

namespace Persistence.Contexts;

public class InslariDbContext(DbContextOptions<InslariDbContext> dbContextOptions, IMediator _mediator)
    : DbContext(dbContextOptions)
{
    public const string DEFAULT_SCHEMA = "dbo";

    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Language> Languages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Language>().HasData(
            new Language { Id = new Guid("7b2df0bb-a235-4a7f-a37c-6018a942f32e"), Code = "en", Name = "English", IsActive = true, IsDefault = true },
            new Language { Id = new Guid("44b4eccb-e0eb-45b2-b925-f1fbf449634d"), Code = "tr", Name = "Türkçe", IsActive = true, IsDefault = false }
        );
    }

    public override int SaveChanges()
    {
        OnBeforeSave();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        OnBeforeSave();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        OnBeforeSave();

        var result = await base.SaveChangesAsync(cancellationToken);

        await PublishDomainEventsAsync();

        return result;
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        OnBeforeSave();

        var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        await PublishDomainEventsAsync();

        return result;
    }

    private void OnBeforeSave()
    {
        var addedEntities = ChangeTracker.Entries()
            .Where(i => i.State == EntityState.Added)
            .Select(i => (BaseEntity)i.Entity);
        PrepareAddedEntities(addedEntities);
    }

    private void PrepareAddedEntities(IEnumerable<BaseEntity> entities)
    {
        foreach (var entity in entities)
        {
            if (entity.IsDeleted == default)
                entity.IsDeleted = false;
            if (entity.CreatedAt == DateTime.MinValue)
                entity.CreatedAt = DateTime.Now;
        }
    }

    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<BaseEntity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.GetDomainEvents();

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
           await _mediator.Publish(domainEvent);
        }
    }
}