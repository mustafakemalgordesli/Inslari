using Domain.Entities;
using Domain.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories.EntityFramework;

public class EfContactRepository(InslariDbContext dbContext) : EfGenericRepository<Contact>(dbContext), IContactRepository
{
}
