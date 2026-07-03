using APIRequestMonitoringSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIRequestMonitoringSystem.Core.Services
{
    public class ApiRequestMonitorService
    {
        private readonly IEnumerable<ApiRequest> _data;
        public ApiRequestMonitorService( IEnumerable<ApiRequest> data)
        {
            this._data = data;
        }

        public int GetTotalNumberOfRequests()
        {
            return _data.Count();
        }

        public int GetTotalNumberOfSuccessfulRequests()
        {
            return _data.Count(a=>a.Success);
        }

        public double GetAvgResponseTime()
        {
            return _data.Where(a => a.Success).Average(a => a.ResponseTimeMs);
        }

        public IEnumerable<(string Service, int TotalRequests, int FailedRequests, double AvgResponseTime)> GetServicePerformance()
        {
            return _data.GroupBy(a => a.Service)
                .Select(a => (Service: a.Key, 
                TotalRequests: a.Count(), 
                FailedRequests: a.Where(b => !b.Success).Count(), 
                AvgResponseTime: a.Where(b => b.Success).Average(c=> c.ResponseTimeMs)));
        }

        public IEnumerable<(string Region, double SuccessRate)> GetRegionReliability()
        {
            return _data.GroupBy(a => a.Region)
                .Select(a => (Region: a.Key, SuccessRate: Math.Round( (double) a.Where(b=>b.Success).Count() / a.Count() * 100, 2) ));
        }

        public IEnumerable<(string Service, int ResponseTimeMs)> GetLatestRequestPerService()
        {
            return _data.GroupBy(a => a.Service)
                .Select(a => (Service: a.Key, ResponseTimeMs: a.OrderByDescending(b => b.Timestamp).First().ResponseTimeMs));
        }

        public string GetTopServiceByLoad()
        {
            var temp = _data.Where(a => a.Success).GroupBy(a => a.Service).Select(a => (Service: a.Key, TotalReponseTime: a.Sum(b => b.ResponseTimeMs)));
            return temp.OrderByDescending(a => a.TotalReponseTime).First().Service;
        }

        public IEnumerable<string> GetSlowUnreliableServices()
        {
            var temp = _data.GroupBy(a => a.Service)
                .Select(a => (Service: a.Key, SuccessRate: Math.Round((double)a.Where(b => b.Success).Count() / a.Count() * 100, 2), AverageResponseTime: a.Average(b=>b.ResponseTimeMs)));
            return temp.Where(a => a.SuccessRate < 60 || a.AverageResponseTime > 500).Select(a => a.Service);
        }
    }
}
