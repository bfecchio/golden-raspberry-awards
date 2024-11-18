using Awards.Api.Contracts;
using Awards.Api.Tests.SeedWork;
using Awards.Contracts.Producers;
using FluentAssertions;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Awards.Api.Controllers;

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

            //responseContent.Min.Should()
            //    .Contain(r => r.Producer == "Producer 1" && r.Interval == 1).And
            //    .Contain(r => r.Producer == "Producer 2" && r.Interval == 5);

            //responseContent.Max.Should().Contain(r => r.Producer == "Producer 2" && r.Interval == 5);

            // Valida os intervalos mínimos
            responseContent.Min.Should().ContainSingle(r => r.Producer == "Producer 1" && r.Interval == 1 && r.PreviousWin == 2008 && r.FollowingWin == 2009);
            responseContent.Min.Should().ContainSingle(r => r.Producer == "Producer 2" && r.Interval == 5 && r.PreviousWin == 2010 && r.FollowingWin == 2015);

            // Valida os intervalos máximos
            responseContent.Max.Should().ContainSingle(r => r.Producer == "Producer 2" && r.Interval == 5 && r.PreviousWin == 2010 && r.FollowingWin == 2015);
            responseContent.Max.Should().ContainSingle(r => r.Producer == "Producer 1" && r.Interval == 1 && r.PreviousWin == 2008 && r.FollowingWin == 2009);

        }
    }
}
