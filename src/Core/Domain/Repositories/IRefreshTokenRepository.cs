using Domain.Entities;

namespace Domain.Repositories;

public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
{
    Task<RefreshToken?> GetByTokenIncludeUser(string token);
}
