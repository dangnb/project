using MNG.Contract.Abstractions.Shared;
using MediatR;

namespace MNG.Contract.Abstractions.Message;
public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
