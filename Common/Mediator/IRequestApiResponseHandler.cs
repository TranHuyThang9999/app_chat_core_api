using MediatR;

namespace PaymentCoreServiceApi.Common.Mediator;

public interface IRequestApiResponseHandler<TRequest, TResponse> : IRequestHandler<TRequest, ApiResponse<TResponse>>
    where TRequest : IRequestApiResponse<TResponse>
{
}
