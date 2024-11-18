using Awards.Application.Core.Abstractions.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Awards.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddDbContext<AwardsDbContext>(options => options.UseInMemoryDatabase(databaseName: "AwardsDb"));
            services.AddScoped<IDbContext>(serviceProvider => serviceProvider.GetRequiredService<AwardsDbContext>());
            services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<AwardsDbContext>());

            return services;
        }
    }
}
