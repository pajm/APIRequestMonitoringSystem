# API Request Monitoring System

Tracks API requests with metrics like total count, success rate, response time, and courier/region performance. Designed to provide insights into API reliability and throughput.

## Key Components
- **Models**: `ApiRequest` (request data model)
- **Services**: `ApiRequestMonitorService` (analyzes request data)

## Functionality
- Tracks request volume by service and region
- Calculates success rates and average response times
- Identifies slow/unsatisfactory services

## Usage
1. Feed API request data to `ApiRequestMonitorService`
2. Use analysis methods to generate reports
3. Visualize results using the returned metrics
