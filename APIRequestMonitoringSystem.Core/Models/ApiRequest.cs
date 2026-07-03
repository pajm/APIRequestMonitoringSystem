using System;
using System.Collections.Generic;
using System.Text;

namespace APIRequestMonitoringSystem.Core.Models
{
    public record ApiRequest(
    string RequestId,
    string Service,
    string Region,
    int ResponseTimeMs,
    bool Success,
    DateTime Timestamp
);
}
