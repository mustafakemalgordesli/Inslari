using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Repositories.EntityFramework;

public class EfRefreshTokenRepository(InslariDbContext dbContext) : EfGenericRepository<RefreshToken>(dbContext), IRefreshTokenRepository
{
    public async Task<RefreshToken?> GetByTokenIncludeUser(string token)
    {
        return await DbSet
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == token);
    }
}
