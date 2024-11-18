using Awards.Api.Tests.Extensions;
using Awards.Api.Tests.Fixtures;
using Awards.Domain.Entities;
using Awards.Persistence;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Awards.Api.Tests.SeedWork
{
    public class TestHostFixture : WebApplicationFactory<TestStartup>
    {
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveDbContext<AwardsDbContext>();
                services.AddDbContext<AwardsDbContext>(options => options.UseInMemoryDatabase("AwardsTestDb"));
                
                // Inicializa a massa de dados fake
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<AwardsDbContext>();

                db.Database.EnsureCreated();

                db.Set<Movie>().AddRange(MovieFixture.SetFixture());
                db.SaveChanges();

            });

            builder.UseStartup<TestStartup>()
                .UseSolutionRelativeContentRoot("tests")
                .UseTestServer();   
        }
    }

    [CollectionDefinition(nameof(ApiCollection))]
    public class ApiCollection : ICollectionFixture<TestHostFixture>
    { }
}
