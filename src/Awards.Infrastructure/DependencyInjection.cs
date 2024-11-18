using Awards.Application.Core.Abstractions.Importation;
using Awards.Infrastructure.Importation;
using Awards.Infrastructure.Importation.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Awards.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SeedSettings>(configuration.GetSection(SeedSettings.SettingsKey));
            services.AddScoped<ISeedService, SeedService>();

            return services;
        }
    }
}
