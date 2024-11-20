using Awards.Api.Controllers;
using Awards.Api.Tests.SeedWork;
using Awards.Contracts.Producers;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Awards.Api.Tests.Scenarios
{
    [Collection(nameof(ApiCollection))]
    public sealed class ProducerControllerTests
    {
        private readonly TestHostFixture _testHostFixture;

        public ProducerControllerTests(TestHostFixture fixture)
        {
            _testHostFixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async Task GetProducerAwardIntervals_ReturnsCorrectResults()
        {
            // Act
            var response = await _testHostFixture.Server
                .CreateHttpApiRequest<ProducerController>(controller => controller.GetProducerAwardIntervals())
                .GetAsync();
            
            // Assert
            response.Should().BeSuccessful();
            var responseContent = await response.Content.ReadFromJsonAsync<ProducerAwardIntervalsResponse>();
            
            responseContent.Should().NotBeNull();

            responseContent.Min.Should()
                .Contain(r => r.Producer == "Producer 1" && r.Interval == 1).And
                .Contain(r => r.Producer == "Producer 2" && r.Interval == 1);

            responseContent.Max.Should()
                .Contain(r => r.Producer == "Producer 2" && r.Interval == 5);
        }
    }
}
