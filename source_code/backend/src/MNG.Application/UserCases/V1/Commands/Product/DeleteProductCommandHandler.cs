
using AutoMapper;
using MNG.Contract.Abstractions.Message;
using MNG.Contract.Abstractions.Shared;
using MNG.Contract.Services.V1.Product;
using MNG.Domain.Abstractions;
using MNG.Domain.Abstractions.Repositories;
using MediatR;

namespace MNG.Application.UserCases.V1.Commands.Product;
public sealed class DeleteProductCommandHandler : ICommandHandler<Command.DeleteProductCommand>
{
    private readonly IRepositoryBase<Domain.Entities.Product, Guid> _productRepositoryBase;
    private readonly IPublisher _publisher;
    private readonly IUnitOfWork _unitOfWork;


    public DeleteProductCommandHandler(IRepositoryBase<Domain.Entities.Product, Guid> productRepositoryBase, IPublisher publisher, IUnitOfWork unitOfWork)
    {
        _productRepositoryBase = productRepositoryBase;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async Task<Result> Handle(Command.DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepositoryBase.FindByIdAsync(request.Id);
        _productRepositoryBase.Remove(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _publisher.Publish(new DomainEvent.ProductDeleted(Guid.NewGuid()));
        return Result.Success(product);
    }
}
