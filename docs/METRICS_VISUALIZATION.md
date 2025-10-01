# Metrics Visualization Guide

This guide explains how to set up a complete monitoring stack to visualize your application metrics using Prometheus and Grafana.

## Overview

The monitoring stack consists of three components:
1. **Your Application** - Exposes metrics at `http://localhost:5050/metrics`
2. **Prometheus** - Scrapes and stores metrics
3. **Grafana** - Visualizes metrics in dashboards

## Quick Setup with Docker Compose

Create a `docker-compose.monitoring.yml` file in your project root:

```yaml
version: '3.8'

services:
  prometheus:
    image: prom/prometheus:latest
    container_name: prometheus
    ports:
      - "9090:9090"
    volumes:
      - ./prometheus.yml:/etc/prometheus/prometheus.yml
    networks:
      - monitoring

  grafana:
    image: grafana/grafana-enterprise:latest
    container_name: grafana
    ports:
      - "3000:3000"
    environment:
      - GF_SECURITY_ADMIN_USER=admin
      - GF_SECURITY_ADMIN_PASSWORD=admin
    volumes:
      - grafana-storage:/var/lib/grafana
    networks:
      - monitoring
    depends_on:
      - prometheus

volumes:
  grafana-storage:

networks:
  monitoring:
    driver: bridge
```

## Prometheus Configuration

Create a `prometheus.yml` file in your project root:

```yaml
global:
  scrape_interval: 15s

scrape_configs:
  - job_name: 'app-chat-api'
    static_configs:
      - targets: ['host.docker.internal:5050']  # For Windows Docker Desktop
        # For Linux, use your host IP or set up proper networking
        # targets: ['172.17.0.1:5050']
```

## Running the Monitoring Stack

1. Start your application:
```bash
dotnet run
```

2. Start the monitoring stack using one of these methods:

### Method 1: Using provided scripts
```bash
# Windows
start-monitoring.bat

# Linux/Mac
./start-monitoring.sh
```

### Method 2: Manual Docker Compose
```bash
docker-compose -f docker-compose.monitoring.yml up -d
```

3. Access the tools:
   - Prometheus: http://localhost:9090
   - Grafana: http://localhost:3000 (admin/admin)

## Grafana Setup

### 1. Add Prometheus Data Source

1. Log in to Grafana (http://localhost:3000)
2. Go to Configuration → Data Sources
3. Click "Add data source"
4. Select "Prometheus"
5. Set URL to `http://prometheus:9090`
6. Click "Save & Test"

### 2. Create Dashboards

You can either import pre-built dashboards or create your own.

#### Import Pre-built Dashboard

1. Go to Create → Import
2. Enter dashboard ID: 10427 (ASP.NET Core Metrics Dashboard)
3. Select your Prometheus data source
4. Click "Import"

#### Create Custom Dashboard

1. Go to Create → Dashboard
2. Click "Add new panel"
3. Use these queries for different panels:

**Panel 1: Request Rate**
```
rate(http_requests_received_total[5m])
```

**Panel 2: Error Rate**
```
rate(http_requests_received_total{code=~"5.."}[5m])
```

**Panel 3: Request Duration (95th percentile)**
```
histogram_quantile(0.95, sum(rate(http_request_duration_seconds_bucket[5m])) by (le))
```

**Panel 4: Active Requests**
```
http_requests_in_progress
```

**Panel 5: Login Attempts**
```
rate(app_user_login_attempts_total[5m])
```

**Panel 6: File Uploads**
```
rate(app_file_uploads_total[5m])
```

## Useful Prometheus Queries

### Application Health
```promql
# Request success rate
1 - (sum(rate(http_requests_received_total{code=~"5.."}[5m])) / sum(rate(http_requests_received_total[5m])))

# Average request duration
rate(http_request_duration_seconds_sum[5m]) / rate(http_request_duration_seconds_count[5m])

# Requests per second
rate(http_requests_received_total[5m])
```

### Business Metrics
```promql
# Login success rate
app_user_login_attempts_total{status="success"} / ignoring(status) app_user_login_attempts_total

# File upload rate by type
rate(app_file_uploads_total[5m])

# Messages sent rate
rate(app_messages_sent_total[5m])
```

## Alerting Rules

Add these to your `prometheus.yml` for basic alerting:

```yaml
rule_files:
  - "alerts.yml"

# Add this to your prometheus.yml
alerting:
  alertmanagers:
    - static_configs:
        - targets:
          - alertmanager:9093
```

Create an `alerts.yml` file:

```yaml
groups:
- name: app-alerts
  rules:
  - alert: HighErrorRate
    expr: rate(http_requests_received_total{code=~"5.."}[5m]) > 0.05
    for: 2m
    labels:
      severity: warning
    annotations:
      summary: "High error rate (instance {{ $labels.instance }})"
      description: "HTTP error rate is above 5% for more than 2 minutes\n  VALUE = {{ $value }}\n  LABELS = {{ $labels }}"

  - alert: HighLatency
    expr: histogram_quantile(0.95, sum(rate(http_request_duration_seconds_bucket[5m])) by (le)) > 1
    for: 2m
    labels:
      severity: warning
    annotations:
      summary: "High latency (instance {{ $labels.instance }})"
      description: "HTTP request latency is above 1 second\n  VALUE = {{ $value }}\n  LABELS = {{ $labels }}"
```

## Grafana Dashboard JSON

Here's a sample dashboard JSON you can import:

```json
{
  "dashboard": {
    "id": null,
    "title": "App Chat API Metrics",
    "tags": ["aspnetcore", "metrics"],
    "timezone": "browser",
    "schemaVersion": 16,
    "version": 0,
    "refresh": "25s",
    "panels": [
      {
        "id": 1,
        "type": "graph",
        "title": "Request Rate",
        "gridPos": { "x": 0, "y": 0, "w": 12, "h": 8 },
        "targets": [
          {
            "expr": "rate(http_requests_received_total[5m])",
            "legendFormat": "{{method}} {{code}}"
          }
        ]
      },
      {
        "id": 2,
        "type": "graph",
        "title": "Request Duration (95th percentile)",
        "gridPos": { "x": 12, "y": 0, "w": 12, "h": 8 },
        "targets": [
          {
            "expr": "histogram_quantile(0.95, sum(rate(http_request_duration_seconds_bucket[5m])) by (le))",
            "legendFormat": "Duration"
          }
        ]
      }
    ]
  }
}
```

## Best Practices

1. **Dashboard Organization**
   - Group related metrics together
   - Use clear, descriptive panel titles
   - Include units in panel titles or axes

2. **Alerting**
   - Set appropriate thresholds based on historical data
   - Use meaningful alert names and descriptions
   - Avoid alert fatigue with proper grouping

3. **Performance**
   - Don't scrape metrics too frequently (15s is usually good)
   - Use recording rules for expensive queries
   - Monitor your monitoring infrastructure

4. **Security**
   - Secure your Grafana instance with proper authentication
   - Restrict access to Prometheus and Grafana
   - Use HTTPS in production

## Troubleshooting

### Common Issues

1. **Prometheus can't scrape metrics**
   - Check if your app is running on port 5050
   - Verify the target URL in Prometheus configuration
   - Check firewall settings

2. **No data in Grafana**
   - Verify Prometheus data source configuration
   - Check if Prometheus is scraping data successfully
   - Ensure time range is set correctly in Grafana

3. **High cardinality issues**
   - Avoid using high-cardinality labels
   - Monitor metric count in Prometheus
   - Use recording rules to reduce cardinality

### Useful Commands

```bash
# Check if metrics endpoint is accessible
curl http://localhost:5050/metrics

# Check Prometheus targets
curl http://localhost:9090/api/v1/targets

# Check Prometheus metrics
curl http://localhost:9090/metrics
```

This setup will give you a comprehensive view of your application's performance and business metrics through an easy-to-understand web interface.