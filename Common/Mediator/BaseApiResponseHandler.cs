namespace PaymentCoreServiceApi.Common.Mediator;

public abstract class BaseApiResponseHandler<TRequest, TResponse> : IRequestApiResponseHandler<TRequest, TResponse>
    where TRequest : IRequestApiResponse<TResponse>
{
    public abstract Task<ApiResponse<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
}
