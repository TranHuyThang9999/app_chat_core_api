# Metrics Monitoring Guide

This document explains how to use the metrics monitoring system in the application.

## Overview

The application uses `prometheus-net.AspNetCore` library to expose metrics for monitoring. It provides both built-in HTTP metrics and custom application metrics through a middleware-based approach.

## Built-in Metrics

The following HTTP metrics are automatically collected:

- `http_requests_received_total` - Total HTTP requests received
- `http_requests_in_progress` - Current HTTP requests in progress
- `http_request_duration_seconds` - Histogram of HTTP request duration

## Custom Metrics

The application provides the following custom metrics through the [IAppMetricsService](file:///D:/clone_source/app_chat/app_chat_core_api/Services/AppMetricsService.cs#L9-L15):

1. `app_user_login_attempts_total` - Counter for user login attempts (labeled by status: success/failure)
2. `app_messages_sent_total` - Counter for messages sent
3. `app_file_uploads_total` - Counter for file uploads (labeled by file_type)
4. `app_api_calls_total` - Counter for API calls (labeled by controller and action)
5. `app_request_duration_seconds` - Histogram for request duration (labeled by endpoint)

## Middleware-Based Approach

All metrics are collected automatically through the [MetricsMiddleware](file:///D:/clone_source/app_chat/app_chat_core_api/Middlewares/MetricsMiddleware.cs#L7-L76), which means:

1. **No controller modifications needed** - Metrics are collected transparently
2. **Automatic tracking** - Request duration, API calls, and specific events are tracked automatically
3. **Centralized logic** - All metrics logic is in one place, making it easier to maintain

## Adding New Metrics

To add new metrics, modify the [AppMetricsService](file:///D:/clone_source/app_chat/app_chat_core_api/Services/AppMetricsService.cs#L17-L90) class:

1. Add a new metric field (Counter, Gauge, Histogram, etc.)
2. Initialize it in the constructor
3. Add a method to interact with it
4. Register the method in the [IAppMetricsService](file:///D:/clone_source/app_chat/app_chat_core_api/Services/AppMetricsService.cs#L9-L15) interface
5. Update [MetricsMiddleware](file:///D:/clone_source/app_chat/app_chat_core_api/Middlewares/MetricsMiddleware.cs#L7-L76) to track the new metrics where appropriate

Example:
```csharp
// In AppMetricsService constructor
_customCounter = Metrics.CreateCounter(
    "app_custom_counter_total",
    "Description of the counter",
    new CounterConfiguration
    {
        LabelNames = new[] { "label1", "label2" }
    });

// In AppMetricsService class
public void IncrementCustomCounter(string label1, string label2)
{
    _customCounter.WithLabels(label1, label2).Inc();
}

// In IAppMetricsService interface
void IncrementCustomCounter(string label1, string label2);
```

## Accessing Metrics

Metrics are exposed at the `/metrics` endpoint on port 5050:

```
http://localhost:5050/metrics
```

## Prometheus Configuration

To scrape metrics with Prometheus, add this to your `prometheus.yml`:

```yaml
scrape_configs:
  - job_name: 'app-chat-api'
    static_configs:
      - targets: ['your-app-host:5050']
```

## Grafana Dashboard

You can visualize metrics in Grafana by creating panels with Prometheus queries:

- Rate of login attempts: `rate(app_user_login_attempts_total[5m])`
- Total messages sent: `app_messages_sent_total`
- 95th percentile request duration: `histogram_quantile(0.95, sum(rate(app_request_duration_seconds_bucket[5m])) by (le, endpoint))`

## Best Practices

1. Use appropriate metric types:
   - Counter: For values that only increase (e.g., total requests)
   - Gauge: For values that can go up and down (e.g., active users)
   - Histogram: For distributions (e.g., request duration)

2. Use labels sparingly to avoid cardinality explosion

3. Avoid high-cardinality labels like user IDs or request IDs

4. Use consistent naming conventions:
   - Use base units (seconds, bytes)
   - Use plural forms for counters (`_total` suffix)
   - Use descriptive names

5. Instrument at appropriate levels:
   - Business logic level for meaningful metrics
   - Avoid instrumenting every method

6. Leverage middleware for cross-cutting concerns:
   - Request duration tracking
   - API call counting
   - Error rate monitoring