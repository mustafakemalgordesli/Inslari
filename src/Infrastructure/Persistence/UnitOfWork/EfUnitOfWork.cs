using Domain.UnitOfWork;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.Contexts;

namespace Persistence.UnitOfWork;
public class EfUnitOfWork(InslariDbContext context) : IUnitOfWork
{
    protected readonly InslariDbContext _context = context;
    protected bool _disposed;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public virtual void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }

            _disposed = true;
        }
    }

    public virtual async ValueTask DisposeAsync()
    {
        if (!_disposed)
        {
            if (_context != null)
            {
                await _context.DisposeAsync();
            }

            Dispose(disposing: false);

            GC.SuppressFinalize(this);
        }
    }
}

public class EfTransactionUnitOfWork(InslariDbContext context) : EfUnitOfWork(context), ITransactionUnitOfWork
{
    private IDbContextTransaction? _transaction;

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing && _transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }

            base.Dispose(disposing);
        }
    }

    public override async ValueTask DisposeAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        await base.DisposeAsync();
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);

            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            if (_transaction != null)
            {
                await RollbackAsync(cancellationToken);
            }
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }
}


