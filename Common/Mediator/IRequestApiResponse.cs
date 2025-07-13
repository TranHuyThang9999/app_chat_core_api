using MediatR;

namespace PaymentCoreServiceApi.Common.Mediator;

public interface IRequestApiResponse<T> : IRequest<ApiResponse<T>>
{
}