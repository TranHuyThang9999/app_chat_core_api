using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Controllers;
using PaymentCoreServiceApi.Services;

namespace PaymentCoreServiceApi.Middlewares;

public class MetricsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IAppMetricsService _metricsService;

    public MetricsMiddleware(RequestDelegate next, IAppMetricsService metricsService)
    {
        _next = next;
        _metricsService = metricsService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            
            // Extract endpoint information
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                var routePattern = endpoint.DisplayName ?? context.Request.Path;
                _metricsService.RecordRequestDuration(routePattern, stopwatch.Elapsed.TotalSeconds);
                
                // Track API calls by controller and action
                var controllerActionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
                if (controllerActionDescriptor != null)
                {
                    _metricsService.IncrementApiCalls(controllerActionDescriptor.ControllerName, controllerActionDescriptor.ActionName);
                }
                
                // Track specific events based on endpoint
                if (context.Request.Path.StartsWithSegments("/api/file/upload") && context.Response.StatusCode == 200)
                {
                    // Try to get content type from form data for file uploads
                    var contentType = context.Request.Form.Files.Count > 0 
                        ? context.Request.Form.Files[0].ContentType 
                        : "unknown";
                    _metricsService.IncrementFileUploads(contentType);
                }
                
                // Track login attempts
                if (context.Request.Path.StartsWithSegments("/api/auth/login"))
                {
                    var status = context.Response.StatusCode == 200 ? "success" : "failure";
                    _metricsService.IncrementUserLoginAttempts(status);
                }
                
                // Track messages sent
                if (context.Request.Path.StartsWithSegments("/api/messages/send") && context.Response.StatusCode == 200)
                {
                    _metricsService.IncrementMessageSent();
                }
            }
            else
            {
                _metricsService.RecordRequestDuration(context.Request.Path, stopwatch.Elapsed.TotalSeconds);
            }
        }
    }
}