using Domain.Repositories;
using Domain.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Persistence.Repositories.EntityFramework;
using Persistence.UnitOfWork;

namespace Persistence;

public static class ServiceRegistration
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<InslariDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DbConnection")));

        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddScoped<ITransactionUnitOfWork, EfTransactionUnitOfWork>();

        services.AddScoped<IUserRepository, EfUserRepository>();
    }
}
