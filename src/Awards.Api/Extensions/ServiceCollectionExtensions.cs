using Awards.Application.Core.Abstractions.Importation;
using Awards.Domain.Entities;
using Awards.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Awards.Api.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        #region Extension Methods

        internal static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Golden Raspberry Awards API",
                    Version = "v1",
                });
            });

            return services;
        }

        internal static IApplicationBuilder EnsureDatabaseCreated(this IApplicationBuilder builder)
        {
            using (var serviceScope = builder.ApplicationServices.CreateScope())
            {
                var seedService = serviceScope.ServiceProvider.GetRequiredService<ISeedService>();
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<AwardsDbContext>();                

                dbContext.Database.EnsureCreated();
                
                dbContext.Set<Movie>().AddRange(seedService.Load());
                dbContext.SaveChanges();
            }                        

            return builder;
        }

        #endregion
    }
}
