using Prometheus;

namespace PaymentCoreServiceApi.Services;

public interface IAppMetricsService
{
    void IncrementUserLoginAttempts(string status);
    void IncrementMessageSent();
    void RecordRequestDuration(string endpoint, double duration);
    void IncrementFileUploads(string fileType);
    void IncrementApiCalls(string controller, string action);
}

public class AppMetricsService : IAppMetricsService
{
    private readonly Counter _loginAttemptsCounter;
    private readonly Counter _messagesSentCounter;
    private readonly Histogram _requestDurationHistogram;
    private readonly Counter _fileUploadsCounter;
    private readonly Counter _apiCallsCounter;

    public AppMetricsService()
    {
        // Counter for user login attempts
        _loginAttemptsCounter = Metrics.CreateCounter(
            "app_user_login_attempts_total",
            "Total number of user login attempts",
            new CounterConfiguration
            {
                LabelNames = new[] { "status" } // success, failure
            });

        // Counter for messages sent
        _messagesSentCounter = Metrics.CreateCounter(
            "app_messages_sent_total",
            "Total number of messages sent");

        // Histogram for request duration
        _requestDurationHistogram = Metrics.CreateHistogram(
            "app_request_duration_seconds",
            "Histogram of request duration in seconds",
            new HistogramConfiguration
            {
                LabelNames = new[] { "endpoint" },
                Buckets = Histogram.ExponentialBuckets(0.01, 2, 10) // 10ms to ~10s
            });

        // Counter for file uploads
        _fileUploadsCounter = Metrics.CreateCounter(
            "app_file_uploads_total",
            "Total number of file uploads",
            new CounterConfiguration
            {
                LabelNames = new[] { "file_type" }
            });

        // Counter for API calls
        _apiCallsCounter = Metrics.CreateCounter(
            "app_api_calls_total",
            "Total number of API calls",
            new CounterConfiguration
            {
                LabelNames = new[] { "controller", "action" }
            });
    }

    public void IncrementUserLoginAttempts(string status)
    {
        _loginAttemptsCounter.WithLabels(status).Inc();
    }

    public void IncrementMessageSent()
    {
        _messagesSentCounter.Inc();
    }

    public void RecordRequestDuration(string endpoint, double duration)
    {
        _requestDurationHistogram.WithLabels(endpoint).Observe(duration);
    }

    public void IncrementFileUploads(string fileType)
    {
        _fileUploadsCounter.WithLabels(fileType).Inc();
    }

    public void IncrementApiCalls(string controller, string action)
    {
        _apiCallsCounter.WithLabels(controller, action).Inc();
    }
}