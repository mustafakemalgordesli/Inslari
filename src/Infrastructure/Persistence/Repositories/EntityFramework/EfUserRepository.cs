using Domain.Entities;
using Domain.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories.EntityFramework;

public class EfUserRepository(InslariDbContext dbContext) : EfGenericRepository<User>(dbContext), IUserRepository
{
}
