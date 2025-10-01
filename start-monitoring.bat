@echo off
echo Starting monitoring stack with Prometheus and Grafana...
echo.

echo Make sure your application is running on port 5050
echo If not, start it with: dotnet run
echo.

echo Starting Docker containers...
docker-compose -f docker-compose.monitoring.yml up -d

echo.
echo Monitoring stack started successfully!
echo.
echo Access the tools:
echo   - Prometheus: http://localhost:9090
echo   - Grafana: http://localhost:3000 (admin/admin)
echo.
echo Next steps:
echo   1. Log in to Grafana
echo   2. Add Prometheus data source (URL: http://prometheus:9090)
echo   3. Import dashboard or create your own
echo.
pause