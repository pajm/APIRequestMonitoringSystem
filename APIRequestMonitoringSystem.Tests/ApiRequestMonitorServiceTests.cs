using APIRequestMonitoringSystem.Core.Models;
using APIRequestMonitoringSystem.Core.Services;
using FluentAssertions;
using Xunit;

namespace APIRequestMonitoringSystem.Tests.Services
{
    public class ApiRequestMonitorServiceTests
    {
        private readonly ApiRequestMonitorService _service;

        public ApiRequestMonitorServiceTests()
        {
            var data = new List<ApiRequest>
            {
                new("R1", "Auth", "UK", 100, true,  new DateTime(2026,1,1,10,0,0)),
                new("R2", "Auth", "UK", 200, false, new DateTime(2026,1,1,10,1,0)),
                new("R3", "Auth", "UK", 150, true,  new DateTime(2026,1,1,10,2,0)),

                new("R4", "Auth", "US", 300, true,  new DateTime(2026,1,1,10,3,0)),

                new("R5", "Payments", "UK", 50, true,  new DateTime(2026,1,1,10,4,0)),
                new("R6", "Payments", "UK", 80, false, new DateTime(2026,1,1,10,5,0)),

                new("R7", "Payments", "US", 120, true, new DateTime(2026,1,1,10,6,0)),
            };

            _service = new ApiRequestMonitorService(data);
        }

        [Fact]
        public void GetTotalNumberOfRequests_ReturnsCorrectCount()
        {
            _service.GetTotalNumberOfRequests().Should().Be(7);
        }

        [Fact]
        public void GetTotalNumberOfSuccessfulRequests_ReturnsCorrectCount()
        {
            _service.GetTotalNumberOfSuccessfulRequests().Should().Be(5);
        }

        [Fact]
        public void GetAvgResponseTime_ReturnsCorrectAverage()
        {
            var result = _service.GetAvgResponseTime();

            // successful: 100, 150, 300, 50, 120
            result.Should().BeApproximately(144, 0.1);
        }

        [Fact]
        public void GetServicePerformance_ReturnsCorrectMetrics()
        {
            var result = _service.GetServicePerformance().ToList();

            var auth = result.Single(x => x.Service == "Auth");
            auth.TotalRequests.Should().Be(4);
            auth.FailedRequests.Should().Be(1);
            auth.AvgResponseTime.Should().BeApproximately(183.33, 0.1);

            var payments = result.Single(x => x.Service == "Payments");
            payments.TotalRequests.Should().Be(3);
            payments.FailedRequests.Should().Be(1);
            payments.AvgResponseTime.Should().BeApproximately(85, 0.1);
        }

        [Fact]
        public void GetRegionReliability_ReturnsCorrectSuccessRate()
        {
            var result = _service.GetRegionReliability().ToList();

            result.Should().Contain(x =>
                x.Region == "UK" &&
                x.SuccessRate == 60.0
            );

            result.Should().Contain(x =>
                x.Region == "US" &&
                x.SuccessRate == 100.0
            );
        }

        [Fact]
        public void GetLatestRequestPerService_ReturnsMostRecentRequest()
        {
            var result = _service.GetLatestRequestPerService().ToList();

            result.Should().Contain(x =>
                x.Service == "Auth" &&
                x.ResponseTimeMs == 300
            );

            result.Should().Contain(x =>
                x.Service == "Payments" &&
                x.ResponseTimeMs == 120
            );
        }

        [Fact]
        public void GetTopServiceByLoad_ReturnsServiceWithHighestLoad()
        {
            var result = _service.GetTopServiceByLoad();

            result.Should().Be("Auth");
        }

        [Fact]
        public void GetSlowUnreliableServices_ReturnsEmpty_WhenAllServicesAreHealthy()
        {
            var result = _service.GetSlowUnreliableServices().ToList();

            result.Should().BeEmpty();
        }
    }
}