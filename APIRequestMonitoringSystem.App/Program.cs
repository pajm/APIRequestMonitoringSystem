using APIRequestMonitoringSystem.Core.Models;
using APIRequestMonitoringSystem.Core.Services;

var requests = new List<ApiRequest>
{
    new("R001", "AuthService", "North", 120, true,  new DateTime(2026, 6, 29, 9, 0, 0)),
    new("R002", "AuthService", "North", 300, false, new DateTime(2026, 6, 29, 9, 5, 0)),
    new("R003", "PaymentService", "South", 200, true,  new DateTime(2026, 6, 29, 9, 10, 0)),
    new("R004", "PaymentService", "South", 500, true,  new DateTime(2026, 6, 29, 10, 0, 0)),
    new("R005", "OrderService", "East", 1000, false, new DateTime(2026, 6, 29, 9, 15, 0)),
    new("R006", "OrderService", "East", 400, true,  new DateTime(2026, 6, 29, 10, 0, 0)),
    new("R007", "AuthService", "West", 150, true,  new DateTime(2026, 6, 29, 11, 0, 0)),
    new("R008", "PaymentService", "West", 800, false, new DateTime(2026, 6, 29, 11, 5, 0)),
};

var service = new ApiRequestMonitorService(requests);

Console.WriteLine("Basic Stats");
Console.WriteLine($"Total Number of Requests: {service.GetTotalNumberOfRequests()}");
Console.WriteLine($"Total Number of Successful Requests: {service.GetTotalNumberOfSuccessfulRequests()}");
Console.WriteLine($"Avg Reponse Time: {service.GetAvgResponseTime()}ms");

Console.WriteLine("Service Performance");
var performance = service.GetServicePerformance();
foreach(var item in performance)
{
    Console.WriteLine($"{item.Service} - {item.TotalRequests} - {item.FailedRequests} - {item.AvgResponseTime}ms");
}
Console.WriteLine("");

Console.WriteLine("Region Reliability");
var regionReliabilty = service.GetRegionReliability();
foreach (var item in regionReliabilty)
{
    Console.WriteLine($"{item.Region} - {item.SuccessRate}%");
}
Console.WriteLine("");

Console.WriteLine("Latest Request Per Service");
var latestRequestPerService = service.GetLatestRequestPerService();
foreach (var item in latestRequestPerService)
{
    Console.WriteLine($"{item.Service} - {item.ResponseTimeMs}ms");
}
Console.WriteLine("");

Console.WriteLine($"Highest Total Response Time: {service.GetTopServiceByLoad()}");
Console.WriteLine("");

Console.WriteLine("Slow Service");
var slowServices = service.GetSlowUnreliableServices();
foreach (var item in slowServices)
{
    Console.WriteLine($"{item}");
}
Console.WriteLine("");