using MNG.Contract.Abstractions.Shared;
using MediatR;

namespace MNG.Contract.Abstractions.Message;
public interface ICommand : IRequest<Result>
{
}
public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
